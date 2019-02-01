using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Sprint.Modules
{
    public  class ModulePresenter
    {
        private AggregateCatalog catalog = new AggregateCatalog();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModulePresenter"/> class.
        /// </summary>
        public ModulePresenter()
        {
            this.ModuleList = new List<Lazy<IModuleInfo, IModuleAttribute>>();
        }

        /// <summary>
        /// Gets or sets the module list.
        /// </summary>
        /// <value>
        /// The module list.
        /// </value>
        [ImportMany(typeof(IModuleInfo), AllowRecomposition = true)]
        public List<Lazy<IModuleInfo, IModuleAttribute>> ModuleList
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize(Action<AggregateCatalog> action)
        {
            action(catalog);

            CompositionContainer cc = new CompositionContainer(catalog);

            try
            {
                cc.ComposeParts(this); // Do the MEF Magic
            }
            catch (Exception ex)
            {
                string str = "";

                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var loaderExceptions = ((System.Reflection.ReflectionTypeLoadException)ex).LoaderExceptions;

                    foreach (Exception exx in loaderExceptions)
                    {
                        str += ex.Message + "\\n";
                    }

                    throw new Exception(str);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Initialize((c) =>
            {
                c.Catalogs.Add(new DirectoryCatalog(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Consts.ModuleFolder)));
            });
        }

        /// <summary>
        /// Determines whether the specified name is loaded.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool IsLoaded(string name)
        {
            return (GetModuleInstance(name) != null);
        }

        /// <summary>
        /// Gets the module instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IModuleInfo GetModuleInstance(string name)
        {
            IModuleInfo instance = null;
            foreach (var l in ModuleList)
            {
                if (l.Metadata.Name == name)
                {
                    instance = l.Value;
                    break;
                }
            }

            return instance;
        }
    }
}
