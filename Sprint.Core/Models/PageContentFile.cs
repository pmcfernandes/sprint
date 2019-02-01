namespace Sprint.Models
{
    public class PageContentFile : ContentFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageContentFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public PageContentFile(string fileName)
            : base(fileName, "page.cshtml")
        {

        }
    }
}