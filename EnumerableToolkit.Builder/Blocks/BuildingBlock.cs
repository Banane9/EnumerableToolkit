﻿namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents the basic building block of <see cref="IEnumerableBuilder{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public abstract class BuildingBlock<T> : IBuildingBlock<T>
    {
        /// <inheritdoc/>
        public abstract IEnumerable<T> Apply(IEnumerable<T> current);

        IEnumerable<T> IParametrizedBuildingBlock<T, object>.Apply(IEnumerable<T> current, object parameters)
            => Apply(current);
    }

    /// <summary>
    /// Defines the interface for a basic building block
    /// that can be applied to incrementally generate a sequence.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    public interface IBuildingBlock<T> : IParametrizedBuildingBlock<T, object>
    {
        /// <summary>
        /// Applies this <see cref="IEnumerableBuilder{T}"/> building block to the given sequence.
        /// </summary>
        /// <param name="current">The current sequence generated by the builder.</param>
        /// <returns>The new sequence with this building block applied.</returns>
        public IEnumerable<T> Apply(IEnumerable<T> current);
    }

    /// <summary>
    /// Defines the interface for a parametrized building block
    /// that can be applied to incrementally generate a sequence.
    /// </summary>
    /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters for generating a sequence.</typeparam>
    public interface IParametrizedBuildingBlock<T, in TParameters>
    {
        /// <summary>
        /// Applies this <see cref="IParametrizedEnumerableBuilder{T, TParameters}"/> building block to the given sequence.
        /// </summary>
        /// <param name="current">The current sequence generated by the builder.</param>
        /// <param name="parameters">The parameters to generate a sequence with.</param>
        /// <returns>The new sequence with this building block applied.</returns>
        public IEnumerable<T> Apply(IEnumerable<T> current, TParameters parameters);
    }
}