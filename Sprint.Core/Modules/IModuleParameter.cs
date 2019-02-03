using Sprint.Models;

namespace Sprint.Modules
{
    public interface IModuleParameter
    {
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        string Folder
        {
            get;
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        string Filename
        {
            get;
        }

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>
        /// The content file.
        /// </value>
        IContentFile ContentFile
        {
            get;
        }
    }
}
