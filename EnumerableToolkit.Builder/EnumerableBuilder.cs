using EnumerableToolkit.Builder.Blocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Allows constructing an <see cref="IEnumerable{T}">enumerable</see>
    /// sequence incrementally out of <see cref="IBuildingBlock{T}">building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public sealed class EnumerableBuilder<T> : IEnumerableBuilder<T>
    {
        private readonly PrioritySortedCollection<Prioritizable<IBuildingBlock<T>>> _buildingBlocks = [];

        /// <inheritdoc/>
        public IEnumerable<IBuildingBlock<T>> BuildingBlocks => _buildingBlocks.Unwrap();

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(Prioritizable<IBuildingBlock<T>> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params Prioritizable<IBuildingBlock<T>>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<Prioritizable<IBuildingBlock<T>>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void ClearBuildingBlocks() => _buildingBlocks.Clear();

        /// <inheritdoc/>
        public IEnumerable<T> GetEnumerable()
        {
            var current = Enumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks.Unwrap())
                current = block.Apply(current);

            return current;
        }

        /// <inheritdoc/>
        public bool RemoveBuildingBlock(Prioritizable<IBuildingBlock<T>> block)
            => _buildingBlocks.Remove(block);
    }

    /// <summary>
    /// Defines the interface for enumerable builders that
    /// allow constructing an <see cref="IEnumerable{T}">enumerable</see>
    /// sequence incrementally out of <see cref="IBuildingBlock{T}">building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public interface IEnumerableBuilder<T>
    {
        /// <summary>
        /// Gets the current application chain of this enumerable builder.
        /// </summary>
        public IEnumerable<IBuildingBlock<T>> BuildingBlocks { get; }

        /// <summary>
        /// Gets the number of <see cref="IBuildingBlock{T}">building blocks</see>
        /// that have been added to this enumerable builder.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building block</see>
        /// into the prioritized application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(Prioritizable<IBuildingBlock<T>> block);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// into the prioritized application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<Prioritizable<IBuildingBlock<T>>> blocks);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// into the prioritized application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(params Prioritizable<IBuildingBlock<T>>[] blocks);

        /// <summary>
        /// Removes all <see cref="IBuildingBlock{T}">building blocks</see>
        /// from the application chain of this enumerable builder.
        /// </summary>
        public void ClearBuildingBlocks();

        /// <summary>
        /// Constructs the currently constructed sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed enumerable sequence.</returns>
        public IEnumerable<T> GetEnumerable();

        /// <summary>
        /// Removes the first instance of the given <see cref="IBuildingBlock{T}">building block</see>
        /// from the application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to remove.</param>
        /// <returns><c>true</c> if a building block was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveBuildingBlock(Prioritizable<IBuildingBlock<T>> block);
    }
}