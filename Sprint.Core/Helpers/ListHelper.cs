using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Helpers
{
    class ListHelper
    {
        /// <summary>
        /// Determines whether [contains] [the specified items].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified items]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<T>(T[] items, T item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Equals(item))
                    return true;
            }

            return false;
        }
    }
}
