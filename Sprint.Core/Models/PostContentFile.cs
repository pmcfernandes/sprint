namespace Sprint.Models
{
    public class PostContentFile : ContentFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostContentFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public PostContentFile(string fileName)
            : base(fileName, "post.cshtml")
        {

        }
    }
}
