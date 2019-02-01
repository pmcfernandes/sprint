using System.Dynamic;

namespace Sprint
{
    public interface IGenerator
    {
        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        string GenerateOutput(ExpandoObject model, string template);
    }
}