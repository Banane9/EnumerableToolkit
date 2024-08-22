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
        private readonly List<IBuildingBlock<T>> _buildingBlocks = [];

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(IBuildingBlock<T> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<IBuildingBlock<T>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params IBuildingBlock<T>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public IEnumerable<T> GetEnumerable()
        {
            var current = Enumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks)
                block.Apply(current);

            return current;
        }
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
        /// Gets the number of <see cref="IBuildingBlock{T}">building blocks</see>
        /// that have been added to this enumerable builder.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building block</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(IBuildingBlock<T> block);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<IBuildingBlock<T>> blocks);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(params IBuildingBlock<T>[] blocks);

        /// <summary>
        /// Constructs the currently constructed sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed enumerable sequence.</returns>
        public IEnumerable<T> GetEnumerable();
    }
}