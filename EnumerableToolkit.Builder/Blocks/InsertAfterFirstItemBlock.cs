using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that inserts a sequence of items after the first item matching a <see cref="InsertAfter(T, int)">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class InsertAfterFirstItemBlock<T> : AddingBlock<T>
    {
        /// <inheritdoc/>
        protected InsertAfterFirstItemBlock(IEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
        {
            var i = 0;
            var matched = false;

            foreach (var item in current)
            {
                yield return item;

                if (!matched && InsertAfter(item, i++))
                {
                    matched = true;

                    foreach (var insertedItem in sequence)
                        yield return insertedItem;
                }
            }
        }

        /// <summary>
        /// Determines whether this block's additions should be inserted after the given item (and only that).
        /// </summary>
        /// <param name="current">The last item that was returned.</param>
        /// <param name="index">The index of the last item that was returned.</param>
        /// <returns><c>true</c> if the additions should be inserted after the item; otherwise, <c>false</c>.</returns>
        protected abstract bool InsertAfter(T current, int index);
    }
}