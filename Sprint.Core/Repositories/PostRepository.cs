using System.IO;

namespace Sprint.Repositories
{
    public class PostRepository : ContentFileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        public PostRepository()
            : base(Path.Combine(Consts.ContentFolder, Consts.PostFolder))
        {

        }
    }
}
