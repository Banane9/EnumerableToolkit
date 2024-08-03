namespace EnumerableToolkit.Builder.AsyncBlocks
{
    /// <summary>
    /// Represents a building block of the <see cref="AsyncEnumerableBuilder{T}"/>
    /// that filters the current sequence of items using a <see cref="Filter(T, int)">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class AsyncWhereItemBlock<T> : AsyncBuildingBlock<T>
    {
        /// <inheritdoc/>
        public override IAsyncEnumerable<T> Apply(IAsyncEnumerable<T> current)
            => current.Where(Filter);

        /// <summary>
        /// Determines whether the given item passes the filter and gets returned.
        /// </summary>
        /// <param name="current">The item about to be returned.</param>
        /// <param name="index">The index of the item about to be returned.</param>
        /// <returns><c>true</c> if the item should be returned; otherwise, <c>false</c>.</returns>
        protected abstract bool Filter(T current, int index);
    }

    /// <summary>
    /// Represents a building block of the <see cref="AsyncEnumerableBuilder{T}"/>
    /// that filters the current sequence of items using a given <see cref="Func{T1, T2, TResult}">predicate</see>.
    /// </summary>
    /// <inheritdoc/>
    public sealed class AsyncWhereItemLambdaBlock<T> : AsyncWhereItemBlock<T>
    {
        private readonly Func<T, int, bool> _predicate;

        /// <summary>
        /// Creates a new building block that filters the current sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public AsyncWhereItemLambdaBlock(Func<T, int, bool> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc/>
        protected override bool Filter(T current, int index)
            => _predicate(current, index);
    }
}