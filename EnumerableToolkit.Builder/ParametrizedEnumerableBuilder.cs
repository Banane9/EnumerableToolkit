using EnumerableToolkit.Builder.Blocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Defines the interface for enumerable builders that
    /// allow constructing an <see cref="IEnumerable{T}">enumerable</see>
    /// sequence incrementally out of <see cref="BuildingBlock{T}">building blocks</see>
    /// while using parameter object to control generation.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters for generating a sequence.</typeparam>
    public interface IParametrizedEnumerableBuilder<T, TParameters>
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
        public void AddBuildingBlock(IParametrizedBuildingBlock<T, TParameters> block);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(IEnumerable<IParametrizedBuildingBlock<T, TParameters>> blocks);

        /// <summary>
        /// Adds the given <see cref="IBuildingBlock{T}">building blocks</see>
        /// at the end of the application chain of this enumerable builder.
        /// </summary>
        /// <param name="blocks">The building blocks to add.</param>
        public void AddBuildingBlocks(params IParametrizedBuildingBlock<T, TParameters>[] blocks);

        /// <summary>
        /// Gets the currently constructed sequence of this enumerable builder
        /// while using parameter object to control generation.
        /// </summary>
        /// <param name="parameters">The parameters to generate a sequence with.</param>
        /// <returns>The constructed enumerable sequence.</returns>
        public IEnumerable<T> GetEnumerable(TParameters parameters);
    }

    /// <summary>
    /// Allows constructing an <see cref="IEnumerable{T}">enumerable</see> sequence incrementally
    /// out of <see cref="IParametrizedBuildingBlock{T, TParameters}">building blocks</see>
    /// while using parameter object to control generation.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters for generating a sequence.</typeparam>
    public sealed class ParametrizedEnumerableBuilder<T, TParameters> : IParametrizedEnumerableBuilder<T, TParameters>
    {
        private readonly List<IParametrizedBuildingBlock<T, TParameters>> _buildingBlocks = [];

        /// <inheritdoc/>
        public int Count => _buildingBlocks.Count;

        /// <inheritdoc/>
        public void AddBuildingBlock(IParametrizedBuildingBlock<T, TParameters> block)
            => _buildingBlocks.Add(block);

        /// <inheritdoc/>
        public void AddBuildingBlocks(IEnumerable<IParametrizedBuildingBlock<T, TParameters>> blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public void AddBuildingBlocks(params IParametrizedBuildingBlock<T, TParameters>[] blocks)
            => _buildingBlocks.AddRange(blocks);

        /// <inheritdoc/>
        public IEnumerable<T> GetEnumerable(TParameters parameters)
        {
            var current = Enumerable.Empty<T>();

            // Manual iteration rather than .Aggregate to capture
            // the internal state at call time instead of iteration time
            foreach (var block in _buildingBlocks)
                block.Apply(current, parameters);

            return current;
        }
    }
}