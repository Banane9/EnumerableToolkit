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
        private readonly PrioritySortedCollection<Prioritizable<IAsyncBuildingBlock<T>>> _buildingBlocks = [];

        /// <inheritdoc/>
        public IEnumerable<IAsyncBuildingBlock<T>> BuildingBlocks => _buildingBlocks.Unwrap();

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(Prioritizable<IAsyncBuildingBlock<T>> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<Prioritizable<IAsyncBuildingBlock<T>>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params Prioritizable<IAsyncBuildingBlock<T>>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void ClearBuildingBlocks() => _buildingBlocks.Clear();

        /// <inheritdoc/>
        public IAsyncEnumerable<T> GetEnumerable()
        {
            var current = AsyncEnumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks.Unwrap())
                current = block.Apply(current);

            return current;
        }

        /// <inheritdoc/>
        public bool RemoveBuildingBlock(Prioritizable<IAsyncBuildingBlock<T>> block)
            => _buildingBlocks.Remove(block);
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
        /// Gets the current application chain of this enumerable builder.
        /// </summary>
        public IEnumerable<IAsyncBuildingBlock<T>> BuildingBlocks { get; }

        /// <summary>
        /// Gets the number of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// that have been added to this async enumerable builder.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building block</see>
        /// into the prioritized application chain of this async enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(Prioritizable<IAsyncBuildingBlock<T>> block);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// into the prioritized application chain of this async enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<Prioritizable<IAsyncBuildingBlock<T>>> blocks);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// into the prioritized application chain of this async enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(params Prioritizable<IAsyncBuildingBlock<T>>[] blocks);

        /// <summary>
        /// Removes all <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// from the application chain of this async enumerable builder.
        /// </summary>
        public void ClearBuildingBlocks();

        /// <summary>
        /// Constructs the currently constructed async sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed async enumerable sequence.</returns>
        public IAsyncEnumerable<T> GetEnumerable();

        /// <summary>
        /// Removes the first instance of the given <see cref="IAsyncBuildingBlock{T}">async building block</see>
        /// from the application chain of this async enumerable builder.
        /// </summary>
        /// <param name="block">The async building block to remove.</param>
        /// <returns><c>true</c> if a building block was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveBuildingBlock(Prioritizable<IAsyncBuildingBlock<T>> block);
    }
}