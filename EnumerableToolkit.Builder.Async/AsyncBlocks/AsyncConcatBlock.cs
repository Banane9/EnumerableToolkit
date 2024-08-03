namespace EnumerableToolkit.Builder.AsyncBlocks
{
    /// <summary>
    /// Represents a building block of the <see cref="AsyncEnumerableBuilder{T}"/>
    /// that concatenates a sequence of items to the current one.
    /// </summary>
    /// <inheritdoc/>
    public sealed class AsyncConcatBlock<T> : AsyncAddingBlock<T>
    {
        /// <summary>
        /// Creates a new building block that concatenates the given <paramref name="sequence"/>
        /// to the end of the current constructed enumerable sequence.
        /// </summary>
        /// <inheritdoc/>
        public AsyncConcatBlock(IAsyncEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IAsyncEnumerable<T> Apply(IAsyncEnumerable<T> current)
            => current.Concat(sequence);
    }
}