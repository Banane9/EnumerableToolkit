namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that inserts a sequence of items before every item matching a <see cref="InsertBefore(T, int)">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class InsertBeforeEveryItemBlock<T> : AddingBlock<T>
    {
        /// <inheritdoc/>
        protected InsertBeforeEveryItemBlock(IEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
        {
            var i = 0;

            foreach (var item in current)
            {
                if (InsertBefore(item, i++))
                {
                    foreach (var insertedItem in sequence)
                        yield return insertedItem;
                }

                yield return item;
            }
        }

        /// <summary>
        /// Determines whether this block's additions should be inserted before the given item.
        /// </summary>
        /// <param name="current">The item about to be returned.</param>
        /// <param name="index">The index of the item about to be returned.</param>
        /// <returns><c>true</c> if the additions should be inserted before the item; otherwise, <c>false</c>.</returns>
        protected abstract bool InsertBefore(T current, int index);
    }
}