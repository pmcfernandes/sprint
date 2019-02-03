namespace Sprint.Modules
{
    public enum ModuleType
    {
        AssetsFolder,
        AssetsFile,
        ProcessingStarted,
        ProcessingEnded,
        GeneratedFile,
    }

    public interface IModuleAttribute
    {
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        ModuleType Type
        {
            get;
        }
    }
}
