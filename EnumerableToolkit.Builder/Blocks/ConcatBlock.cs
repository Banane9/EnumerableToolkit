namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/>
    /// that concatenates a sequence of items to the current one.
    /// </summary>
    /// <inheritdoc/>
    public sealed class ConcatBlock<T> : AddingBlock<T>
    {
        /// <summary>
        /// Creates a new building block that concatenates the given <paramref name="sequence"/>
        /// to the end of the current constructed enumerable sequence.
        /// </summary>
        /// <inheritdoc/>
        public ConcatBlock(IEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
            => current.Concat(sequence);
    }
}