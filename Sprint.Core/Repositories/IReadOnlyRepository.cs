using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Repositories
{
    public interface IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Filters the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="index">The index.</param>
        /// <param name="size">The size.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, int index, int size, out int total);

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T SingleOrDefault(Func<T, bool> predicate);
    }
}
