using Sprint.Models;

namespace Sprint.Modules
{
    public class ModuleParameter : IModuleParameter
    {
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public string Folder
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>
        /// The content file.
        /// </value>
        public IContentFile ContentFile
        {
            get;
            internal set;
        }
    }
}
