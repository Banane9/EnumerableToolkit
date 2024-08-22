using EnumerableToolkit.Builder.AsyncBlocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Allows constructing an <see cref="IAsyncEnumerable{T}">async enumerable</see>
    /// sequence incrementally out of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
    public sealed class AsyncEnumerableBuilder<T> : IAsyncEnumerableBuilder<T>
    {
        private readonly List<IAsyncBuildingBlock<T>> _buildingBlocks = [];

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(IAsyncBuildingBlock<T> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<IAsyncBuildingBlock<T>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params IAsyncBuildingBlock<T>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
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

    /// <summary>
    /// Defines the interface for async enumerable builders that
    /// allow constructing an <see cref="IAsyncEnumerable{T}">async enumerable</see>
    /// sequence incrementally out of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
    public interface IAsyncEnumerableBuilder<T>
    {
        /// <summary>
        /// Gets the number of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// that have been added to this enumerable builder.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building block</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(IAsyncBuildingBlock<T> block);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<IAsyncBuildingBlock<T>> blocks);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(params IAsyncBuildingBlock<T>[] blocks);

        /// <summary>
        /// Constructs the currently constructed async sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed async enumerable sequence.</returns>
        public IAsyncEnumerable<T> GetEnumerable();
    }
}