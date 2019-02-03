using Sprint.Helpers;
using Sprint.IO;
using System.IO;

namespace Sprint
{
    public class AssetsCopier
    {
        /// <summary>
        /// The ignored extensions
        /// </summary>
        private string[] ignoredExtensions = new string[] {
            ".less",
            ".sass",
            ".ts",
            ".d.ts",
            ".spec.ts",
            ".map.js"
        };

        /// <summary>
        /// The assets dir
        /// </summary>
        private string _assetsDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetsCopier"/> class.
        /// </summary>
        /// <param name="assetsDir">The assets dir.</param>
        public AssetsCopier(string assetsDir)
        {
            _assetsDir = assetsDir;
        }

        /// <summary>
        /// Gets the assets folder.
        /// </summary>
        /// <value>
        /// The assets folder.
        /// </value>
        public string AssetsFolder
        {
            get { return _assetsDir; }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="outputDir">The output dir.</param>
        public void CopyTo(string outputDir, bool removeIgnoredExtensions = true)
        {
            Directories.CopyDirectory(_assetsDir, outputDir, true);

            if (removeIgnoredExtensions)
            {
                foreach (var fileInfo in Directories.FileList(Path.Combine(outputDir, Consts.AssetsFolder), true))
                {
                    if (ListHelper.Contains<string>(ignoredExtensions, fileInfo.Extension))
                    {
                        Files.Delete(fileInfo.FullName);
                    }
                }
            }
        }
    }
}
