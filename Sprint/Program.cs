using RazorEngine.Templating;
using Sprint.Console;
using Sprint.Generators;
using Sprint.IO;
using Sprint.Models;
using Sprint.Modules;
using Sprint.Repositories;
using System;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace Sprint
{
    class Program
    {
        /// <summary>
        /// Gets or sets the template folder.
        /// </summary>
        /// <value>
        /// The template folder.
        /// </value>
        public static string TemplateFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output folder.
        /// </summary>
        /// <value>
        /// The output folder.
        /// </value>
        public static string OutputFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Runs the in second application domain.
        /// </summary>
        /// <returns></returns>
        private static int RunInSecondAppDomain()
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var current = AppDomain.CurrentDomain;
            var strongNames = new StrongName[0];

            var domain = AppDomain.CreateDomain(
                "MyMainDomain", null,
                current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                strongNames);

            return domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static int Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                return RunInSecondAppDomain();
            }

            System.Console.WriteLine("Sprint v.1.0 - Static Site Generator for Razor and markdown");
            System.Console.WriteLine("Created by Pedro Fernandes @ Patreo");
            System.Console.WriteLine("-----------------------------------------------------------");
            System.Console.WriteLine("");
            System.Console.WriteLine("Command line parameters:");
            System.Console.WriteLine("  -o | -out: Output folder");
            System.Console.WriteLine("  -t | -templates: Template folder where contains razor and assets files");
            System.Console.WriteLine("");

            CommandLineArgs _args = new CommandLineArgs(args);

            if (_args.TryGetValue("out", out string outputDir))
            {
                OutputFolder = outputDir;
            }
            else
            {
                OutputFolder = Consts.OutputFolder;
            }

            if (_args.TryGetValue("template", out string templateDir))
            {
                TemplateFolder = templateDir;
            }
            else
            {
                TemplateFolder = Consts.TemplateFolder;
            }

            ModulePresenter presenter = new ModulePresenter();
            presenter.Initialize();

            // Create output folder
            System.Console.Write("Creating output folder... ");
            Directories.CreateDirectory(OutputFolder);
            System.Console.WriteLine(Files.GetFileInfo(OutputFolder).FullName);

            // Copy all assets to output folder
            System.Console.Write("Copying assets folder to output folder... ");
            AssetsCopier copier = new AssetsCopier(Path.Combine(TemplateFolder, Consts.AssetsFolder));

            foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.AssetsFolder))
            {
                try
                {
                    module.Execute(new ModuleParameter
                    {
                        Folder = copier.AssetsFolder
                    });
                }
                catch (Exception)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Failed at {module.GetType().FullName} running { copier.AssetsFolder}");
                    System.Console.WriteLine("");
                }
            }

            copier.CopyTo(Path.Combine(OutputFolder, Consts.AssetsFolder));

            foreach (var fileInfo in Directories.FileList(Path.Combine(OutputFolder, Consts.AssetsFolder)))
            {
                foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.AssetsFile))
                {
                    try
                    {
                        module.Execute(new ModuleParameter
                        {
                            Folder = fileInfo.Directory.FullName,
                            Filename = fileInfo.FullName
                        });
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine("");
                        System.Console.WriteLine($"Failed at {module.GetType().FullName} running {fileInfo.FullName}");
                        System.Console.WriteLine("");
                    }
                }
            }

            System.Console.WriteLine(System.IO.Path.Combine(Files.GetFileInfo(OutputFolder).FullName, Consts.AssetsFolder));

            // Initialize render
            System.Console.WriteLine("Starting rendering content...");
            IGenerator generator = new RazorGenerator(new DelegateTemplateManager(name =>
            {
                return Files.ReadAllText(Path.Combine(TemplateFolder, name));
            }));

            // Generate pages and posts
            foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.ProcessingStarted))
            {
                try
                {
                    module.Execute(new ModuleParameter
                    {
                        Folder = OutputFolder
                    });
                }
                catch (Exception)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Failed at {module.GetType().FullName} running {OutputFolder}");
                    System.Console.WriteLine("");

                }
            }

            GenerateOutput(presenter, generator, new GenericRepository());
            GenerateOutput(presenter, generator, new PageRepository());
            GenerateOutput(presenter, generator, new PostRepository());

            foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.ProcessingEnded))
            {
                try
                {
                    module.Execute(new ModuleParameter
                    {
                        Folder = OutputFolder
                    });
                }
                catch (Exception)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine($"Failed at {module.GetType().FullName} running {OutputFolder}");
                    System.Console.WriteLine("");
                }
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("Done.");
            System.Console.WriteLine("Thanks for your patience.");

            return 0;
        }

        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        private static void GenerateOutput<T>(ModulePresenter presenter, IGenerator generator, T repository) where T : IRepository<IContentFile>
        {
            foreach (var page in repository.All())
            {
                string strTemplatePath = Path.Combine(TemplateFolder, page.Template);

                if (File.Exists(strTemplatePath) == false)
                {
                    continue;
                }

                string template = Files.ReadAllText(strTemplatePath);

                if (String.IsNullOrEmpty(template))
                {
                    return;
                }

                FileInfo fileInfo = new FileInfo(page.Filename);
                string strOutputPath = Path.Combine(OutputFolder, fileInfo.Name.Replace(".md", ".html"));

                dynamic d = new ExpandoObject();
                d.Title = page.Title;
                d.Slug = page.Slug;
                d.Permalink = page.Permalink;
                d.Date = page.Date;
                d.Author = page.Author;
                d.Excerpt = page.Excerpt;
                d.Content = page.ToHtml();

                string output = generator.GenerateOutput(d, template);

                if (String.IsNullOrEmpty(output))
                {
                    return;
                }
                else
                {
                    System.Console.Write("  Writing " + strOutputPath + "... ");
                    Files.WriteAllText(strOutputPath, output, false);

                    foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.GeneratedFile))
                    {
                        try
                        {
                            module.Execute(new ModuleParameter
                            {
                                Folder = OutputFolder,
                                Filename = strOutputPath,
                                ContentFile = page
                            });
                        }
                        catch (Exception)
                        {
                            System.Console.WriteLine("");
                            System.Console.WriteLine($"Failed at {module.GetType().FullName} running {strOutputPath}");
                            System.Console.WriteLine("");
                        }
                    }

                    System.Console.WriteLine("OK.");
                }
            }
        }
    }
}
