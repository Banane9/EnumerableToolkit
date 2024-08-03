namespace EnumerableToolkit.Builder.AsyncBlocks
{
    /// <summary>
    /// Represents a building block of the <see cref="AsyncEnumerableBuilder{T}"/> that adds a sequence of items.
    /// </summary>
    /// <inheritdoc/>
    public abstract class AsyncAddingBlock<T> : AsyncBuildingBlock<T>
    {
        /// <summary>
        /// The <see cref="IAsyncEnumerable{T}">async enumerable</see> sequence that should be added.
        /// </summary>
        protected readonly IAsyncEnumerable<T> sequence;

        /// <summary>
        /// Creates a new instance of this building block with the given items.
        /// </summary>
        /// <param name="sequence">A sequence of items that should be added.</param>
        protected AsyncAddingBlock(IAsyncEnumerable<T> sequence)
        {
            this.sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));
        }
    }
}