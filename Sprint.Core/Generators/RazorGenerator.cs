using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Dynamic;

namespace Sprint.Generators
{
    public class RazorGenerator : IGenerator
    {
        private IRazorEngineService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorGenerator" /> class.
        /// </summary>
        /// <param name="templateFolder">The template folder.</param>
        public RazorGenerator(string templateFolder)
            : this(new ResolvePathTemplateManager(new string[] { templateFolder }))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorGenerator" /> class.
        /// </summary>
        /// <param name="templateManager">The template manager.</param>
        public RazorGenerator(ITemplateManager templateManager)
        {
            service = RazorEngineService.Create(new TemplateServiceConfiguration()
            {
                Language = RazorEngine.Language.CSharp,
                EncodedStringFactory = new RawStringFactory(),
                TemplateManager = templateManager
            });
        }

        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public string GenerateOutput(ExpandoObject model, string template)
        {
            return service.RunCompile(template, Guid.NewGuid().ToString("N"), null, model);
        }
    }
}