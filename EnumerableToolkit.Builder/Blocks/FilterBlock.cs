using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that filters the current sequence of items using a <see cref="Filter(T, int)">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class FilterBlock<T> : BuildingBlock<T>
    {
        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
            => current.Where(Filter);

        /// <summary>
        /// Determines whether the given item passes the filter and gets returned.
        /// </summary>
        /// <param name="current">The item about to be returned.</param>
        /// <param name="index">The index of the item about to be returned.</param>
        /// <returns><c>true</c> if the item should be returned; otherwise, <c>false</c>.</returns>
        protected abstract bool Filter(T current, int index);
    }
}