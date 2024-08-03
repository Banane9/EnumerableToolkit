using EnumerableToolkit.Builder.Blocks;
using System.Collections;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Allows constructing an <see cref="IEnumerable{T}">enumerable</see>
    /// sequence incrementally out of <see cref="BuildingBlock{T}">building blocks</see>.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public sealed class EnumerableBuilder<T>
    {
        private readonly List<BuildingBlock<T>> _buildingBlocks = [];

        /// <summary>
        /// Gets the number of <see cref="BuildingBlock{T}">building blocks</see>
        /// that have been added to this enumerable builder.
        /// </summary>
        public int Count => _buildingBlocks.Count;

        /// <summary>
        /// Adds the given <see cref="BuildingBlock{T}">building block</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="block">The building block to add.</param>
        public void AddBuildingBlock(BuildingBlock<T> block)
            => _buildingBlocks.Add(block);

        /// <summary>
        /// Adds the given <see cref="BuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<BuildingBlock<T>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <summary>
        /// Constructs the currently constructed sequence of this enumerable builder.
        /// </summary>
        /// <returns>The constructed enumerable sequence.</returns>
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
}