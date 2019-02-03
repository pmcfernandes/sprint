namespace Sprint.Repositories
{
    public class GenericRepository : ContentFileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        public GenericRepository()
            : base(Consts.ContentFolder)
        {

        }
    }
}
