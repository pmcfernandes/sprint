using System.IO;

namespace Sprint.Repositories
{
    public class PageRepository : ContentFileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        public PageRepository()
            : base(Path.Combine(Consts.ContentFolder, Consts.PageFolder))
        {

        }
    }
}
