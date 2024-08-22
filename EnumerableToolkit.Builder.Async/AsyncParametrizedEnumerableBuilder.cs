using EnumerableToolkit.Builder.AsyncBlocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Allows constructing an <see cref="IAsyncEnumerable{T}">async enumerable</see> sequence incrementally
    /// out of <see cref="IAsyncParametrizedBuildingBlock{T, TParameters}">async building blocks</see>
    /// while using parameter object to control generation.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
    public sealed class AsyncParametrizedEnumerableBuilder<T, TParameters> : IAsyncParametrizedEnumerableBuilder<T, TParameters>
    {
        private readonly List<IAsyncParametrizedBuildingBlock<T, TParameters>> _buildingBlocks = [];

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(IAsyncParametrizedBuildingBlock<T, TParameters> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<IAsyncParametrizedBuildingBlock<T, TParameters>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params IAsyncParametrizedBuildingBlock<T, TParameters>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void ClearBuildingBlocks() => _buildingBlocks.Clear();

        /// <inheritdoc/>
        public IAsyncEnumerable<T> GetEnumerable(TParameters parameters)
        {
            var current = AsyncEnumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks)
                block.Apply(current, parameters);

            return current;
        }

        /// <inheritdoc/>
        public bool RemoveBuildingBlock(IAsyncParametrizedBuildingBlock<T, TParameters> block)
            => _buildingBlocks.Remove(block);
    }

    /// <summary>
    /// Defines the interface for async enumerable builders that
    /// allow constructing an <see cref="IAsyncEnumerable{T}">async enumerable</see>
    /// sequence incrementally out of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
    /// while using parameter object to control generation.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
    public interface IAsyncParametrizedEnumerableBuilder<T, TParameters>
    {
        /// <summary>
        /// Gets the number of <see cref="IAsyncBuildingBlock{T}">async building blocks</see>
        /// that have been added to this async enumerable builder.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async parametrized building block</see>
        /// at the end of the application chain of this async enumerable builder.
        /// </summary>
        /// <param name="block">The async parametrized building block to add.</param>
        public void AddBuildingBlock(IAsyncParametrizedBuildingBlock<T, TParameters> block);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async parametrized building blocks</see>
        /// at the end of the application chain of this async enumerable builder.
        /// </summary>
        /// <param name="blocks">The async parametrized building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<IAsyncParametrizedBuildingBlock<T, TParameters>> blocks);

        /// <summary>
        /// Adds the given <see cref="IAsyncBuildingBlock{T}">async parametrized building blocks</see>
        /// at the end of the application chain of this async enumerable builder.
        /// </summary>
        /// <param name="blocks">The async parametrized building blocks to add.</param>
        public void AddBuildingBlocks(params IAsyncParametrizedBuildingBlock<T, TParameters>[] blocks);

        /// <summary>
        /// Removes all <see cref="IAsyncParametrizedBuildingBlock{T, TParameters}">async parametrized building blocks</see>
        /// from the application chain of this async enumerable builder.
        /// </summary>
        public void ClearBuildingBlocks();

        /// <summary>
        /// Gets the currently constructed async sequence of this async enumerable builder
        /// while using parameter object to control generation.
        /// </summary>
        /// <param name="parameters">The parameters to generate an async sequence with.</param>
        /// <returns>The constructed async enumerable sequence.</returns>
        public IAsyncEnumerable<T> GetEnumerable(TParameters parameters);

        /// <summary>
        /// Removes the first instance of the given <see cref="IAsyncParametrizedBuildingBlock{T, TParameters}">async parametrized building block</see>
        /// from the application chain of this async enumerable builder.
        /// </summary>
        /// <param name="block">The async parametrized building block to remove.</param>
        /// <returns><c>true</c> if a building block was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveBuildingBlock(IAsyncParametrizedBuildingBlock<T, TParameters> block);
    }
}