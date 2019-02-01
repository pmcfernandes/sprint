using RazorEngine.Templating;
using Sprint.Console;
using Sprint.Generators;
using Sprint.IO;
using Sprint.Models;
using Sprint.Repositories;
using System;
using System.Dynamic;
using System.IO;

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
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            CommandLineArgs _args = new CommandLineArgs(args);

            if (_args.TryGetValue("out", out string outputDir))
            {
                OutputFolder = outputDir;
            }
            else
            {
                OutputFolder = Consts.TemplateFolder;
            }

            if (_args.TryGetValue("template", out string templateDir))
            {
                TemplateFolder = templateDir;
            }
            else
            {
                TemplateFolder = Consts.TemplateFolder;
            }

            // Create output folder
            Directories.CreateDirectory(OutputFolder);

            // Copy all assets to output folder
            AssetsCopier copier = new AssetsCopier(Path.Combine(TemplateFolder, Consts.AssetsFolder));
            copier.CopyTo(OutputFolder);

            // Initialize render
            IGenerator generator = new RazorGenerator(new DelegateTemplateManager(name =>
            {
                return Files.ReadAllText(Path.Combine(TemplateFolder, name));
            }));

            // Generate pages and posts
            GenerateOutput(generator, new PageRepository());
            GenerateOutput(generator, new PostRepository());
        }

        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        private static void GenerateOutput<T>(IGenerator generator, T repository) where T : IRepository<IContentFile>
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
                    Files.WriteAllText(strOutputPath, output, false);
                }
            }
        }
    }
}
