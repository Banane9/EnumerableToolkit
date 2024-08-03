using EnumerableToolkit.Builder.AsyncBlocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Allows constructing an <see cref="IAsyncEnumerable{T}">enumerable</see>
    /// sequence incrementally out of <see cref="AsyncBuildingBlock{T}">building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public sealed class AsyncEnumerableBuilder<T>
    {
        private readonly List<AsyncBuildingBlock<T>> _buildingBlocks = [];

        /// <summary>
        /// Gets the number of <see cref="AsyncBuildingBlock{T}">building blocks</see>
        /// that have been added to this enumerable builder.
        /// </summary>
        public int Count => _buildingBlocks.Count;

        /// <summary>
        /// Adds the given <see cref="AsyncBuildingBlock{T}">building block</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(AsyncBuildingBlock<T> block)
            => _buildingBlocks.Add(block);

        /// <summary>
        /// Adds the given <see cref="AsyncBuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<AsyncBuildingBlock<T>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <summary>
        /// Constructs the currently constructed async sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed async enumerable sequence.</returns>
        public IAsyncEnumerable<T> GetEnumerable()
        {
            var current = AsyncEnumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks)
                block.Apply(current);

            return current;
        }
    }
}