namespace Sprint.Modules
{
  
    public interface IModuleInfo
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title
        {
            get;
        }

        /// <summary>
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        void Execute(IModuleParameter @params);
    }
}
