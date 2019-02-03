using System;
using System.ComponentModel.Composition;

namespace Sprint.Modules
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute : ExportAttribute, IModuleAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAttibute" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        public ModuleAttribute(string name, ModuleType type)
            : base(typeof(IModuleInfo))
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ModuleType Type
        {
            get;
            private set;
        }
    }
}
