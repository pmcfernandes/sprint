namespace Sprint.Models
{
    class ContentFileData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFileData"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="status">The status.</param>
        public ContentFileData(IContentFile file, ContentFileStatus status = ContentFileStatus.Added)
        {
            ContentFile = file;
            Status = status;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ContentFileStatus Status
        {
            get;
            set;
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
            set;
        }
    }
}
