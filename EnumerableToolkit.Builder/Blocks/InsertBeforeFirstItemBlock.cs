﻿namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that inserts a sequence of items before the first item matching a <see cref="InsertBefore(T, int)">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class InsertBeforeFirstItemBlock<T> : AddingBlock<T>
    {
        /// <inheritdoc/>
        protected InsertBeforeFirstItemBlock(IEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
        {
            var i = 0;
            var matched = false;

            foreach (var item in current)
            {
                if (!matched && InsertBefore(item, i++))
                {
                    matched = true;

                    foreach (var insertedItem in sequence)
                        yield return insertedItem;
                }

                yield return item;
            }
        }

        /// <summary>
        /// Determines whether this block's additions should be inserted before the given item (and only that).
        /// </summary>
        /// <param name="current">The item about to be returned.</param>
        /// <param name="index">The index of the item about to be returned.</param>
        /// <returns><c>true</c> if the additions should be inserted before the item; otherwise, <c>false</c>.</returns>
        protected abstract bool InsertBefore(T current, int index);
    }

    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that inserts a sequence of items before the first item matching a given <see cref="Func{T1, T2, TResult}">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public sealed class InsertBeforeFirstItemLambdaBlock<T> : InsertBeforeFirstItemBlock<T>
    {
        private readonly Func<T, int, bool> _predicate;

        /// <summary>
        /// Creates a new building block that inserts the given <paramref name="sequence"/>
        /// before the first item that matches the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public InsertBeforeFirstItemLambdaBlock(Func<T, int, bool> predicate, IEnumerable<T> sequence) : base(sequence)
        {
            _predicate = predicate;
        }

        /// <inheritdoc/>
        protected override bool InsertBefore(T current, int index)
            => _predicate(current, index);
    }
}