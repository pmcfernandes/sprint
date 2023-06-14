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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace Sprint
{
    class Program
    {
        class Folder
        {
            /// <summary>
            /// Gets or sets the template folder.
            /// </summary>
            /// <value>
            /// The template folder.
            /// </value>
            public string TemplateFolder
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
            public string OutputFolder
            {
                get;
                set;
            }
        }


        /// <summary>
        /// Runs the in second application domain.
        /// </summary>
        /// <returns></returns>
        private static int RunInSecondAppDomain(string[] args)
        {
            AppDomainSetup adSetup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            };

            var domain = AppDomain.CreateDomain("MyMainDomain", null, adSetup, new PermissionSet(PermissionState.Unrestricted), new StrongName[0]);
            return domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location, args);
        }

        /// <summary>
        /// Prints the header.
        /// </summary>
        static void PrintHeader()
        {
            System.Console.WriteLine("Sprint v.1.1 - Static Site Generator for Razor and markdown");
            System.Console.WriteLine("Created by Pedro Fernandes @ https://github.com/pmcfernandes/sprint");
            System.Console.WriteLine("-----------------------------------------------------------");
            System.Console.WriteLine("");
            System.Console.WriteLine("Command line parameters:");
            System.Console.WriteLine("  -o | --out: Output folder");
            System.Console.WriteLine("  -t | --templates: Template folder where contains razor and assets files");
            System.Console.WriteLine("");
        }

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static int Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                return RunInSecondAppDomain(args);
            }

            PrintHeader();

            Folder folders = new Folder();
            CommandLineArgs _args = new CommandLineArgs(args);

            if (_args.TryGetValue("out", out string outputDir))
            {
                folders.OutputFolder = outputDir;
            }
            else
            {
                folders.OutputFolder = Consts.OutputFolder;
            }

            if (_args.TryGetValue("template", out string templateDir))
            {
                folders.TemplateFolder = templateDir;
            }
            else
            {
                folders.TemplateFolder = Consts.TemplateFolder;
            }

            ModulePresenter presenter = new ModulePresenter();
            presenter.Initialize();

            // Create output folder
            System.Console.Write("Creating output folder... ");
            Directories.CreateDirectory(folders.OutputFolder);
            System.Console.WriteLine(Files.GetFileInfo(folders.OutputFolder).FullName);

            // Copy all assets to output folder
            System.Console.Write("Copying assets folder to output folder... ");
            AssetsCopier copier = new AssetsCopier(Path.Combine(folders.TemplateFolder, Consts.AssetsFolder));

            ProcessFolders(presenter, copier);
            copier.CopyTo(Path.Combine(folders.OutputFolder, Consts.AssetsFolder));

            ProcessFiles(presenter, folders);
            System.Console.WriteLine(System.IO.Path.Combine(Files.GetFileInfo(folders.OutputFolder).FullName, Consts.AssetsFolder));

            ProcessingStart(presenter, folders);
            GenerateOutputs(presenter, folders);
            ProcessingEnd(presenter, folders);

            System.Console.WriteLine("");
            System.Console.WriteLine("Done.");
            System.Console.WriteLine("Thanks for your patience.");

            return 0;
        }

        /// <summary>
        /// Generates the outputs.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <param name="folders">The folders.</param>
        private static void GenerateOutputs(ModulePresenter presenter, Folder folders)
        {
            // Initialize render
            System.Console.WriteLine("Starting rendering content...");
            IGenerator generator = new RazorGenerator(new DelegateTemplateManager(name =>
            {
                return Files.ReadAllText(Path.Combine(folders.TemplateFolder, name));
            }));

            IRepository<IContentFile>[] repositories = new IRepository<IContentFile>[] {
                new GenericRepository(),
                new PageRepository(),
                new PostRepository()
            };

            foreach (var repo in repositories)
            {
                GenerateOutput(presenter, generator, repo, folders);
            }
        }

        /// <summary>
        /// Processes the folders.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <param name="copier">The copier.</param>
        private static void ProcessFolders(ModulePresenter presenter, AssetsCopier copier)
        {
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
                    System.Console.WriteLine($"\nFailed at {module.GetType().FullName} running {copier.AssetsFolder} \n");
                }
            }
        }

        /// <summary>
        /// Processes the files.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <param name="folders">The folders.</param>
        private static void ProcessFiles(ModulePresenter presenter, Folder folders)
        {
            foreach (var fileInfo in Directories.FileList(Path.Combine(folders.OutputFolder, Consts.AssetsFolder)))
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
                        System.Console.WriteLine($"\nFailed at {module.GetType().FullName} running {fileInfo.FullName} \n");
                    }
                }
            }
        }

        /// <summary>
        /// Processings the start.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <param name="folders">The folders.</param>
        private static void ProcessingStart(ModulePresenter presenter, Folder folders)
        {
            // Generate pages and posts
            foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.ProcessingStarted))
            {
                try
                {
                    module.Execute(new ModuleParameter
                    {
                        Folder = folders.OutputFolder
                    });
                }
                catch (Exception)
                {
                    System.Console.WriteLine($"\nFailed at {module.GetType().FullName} running {folders.OutputFolder} \n");
                }
            }
        }

        /// <summary>
        /// Processings the end.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <param name="folders">The folders.</param>
        private static void ProcessingEnd(ModulePresenter presenter, Folder folders)
        {
            foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.ProcessingEnded))
            {
                try
                {
                    module.Execute(new ModuleParameter
                    {
                        Folder = folders.OutputFolder
                    });
                }
                catch (Exception)
                {
                    System.Console.WriteLine($"\nFailed at {module.GetType().FullName} running {folders.OutputFolder} \n");
                }
            }
        }

        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        private static void GenerateOutput<T>(ModulePresenter presenter, IGenerator generator, T repository, Folder folders) where T : IRepository<IContentFile>
        {
            foreach (var page in repository.All())
            {
                string strTemplatePath = Path.Combine(folders.TemplateFolder, page.Template);

                if (File.Exists(strTemplatePath) == false)
                {
                    continue;
                }

                string template = Files.ReadAllText(strTemplatePath);

                if (!String.IsNullOrEmpty(template))
                {
                    FileInfo fileInfo = new FileInfo(page.Filename);
                    string strOutputPath = Path.Combine(folders.OutputFolder, fileInfo.Name.Replace(".md", ".html"));

                    dynamic d = new ExpandoObject();
                    d.Title = page.Title;
                    d.Slug = page.Slug;
                    d.Permalink = page.Permalink;
                    d.Date = page.Date;
                    d.Author = page.Author;
                    d.Excerpt = page.Excerpt;
                    d.Content = page.ToHtml();

                    string output = generator.GenerateOutput(d, template);

                    if (!String.IsNullOrEmpty(output))
                    {
                        System.Console.Write("  Writing " + strOutputPath + "... ");
                        Files.WriteAllText(strOutputPath, output, false);

                        foreach (IModuleInfo module in presenter.GetModulesByType(ModuleType.GeneratedFile))
                        {
                            try
                            {
                                module.Execute(new ModuleParameter
                                {
                                    Folder = folders.OutputFolder,
                                    Filename = strOutputPath,
                                    ContentFile = page
                                });
                            }
                            catch (Exception)
                            {
                                System.Console.WriteLine($"\nFailed at {module.GetType().FullName} running {strOutputPath} \n");
                            }
                        }

                        System.Console.WriteLine("OK.");
                    }
                }
            }
        }
    }
}
