namespace EnumerableToolkit.Builder.Blocks
{
    /// <summary>
    /// Represents a building block of the <see cref="EnumerableBuilder{T}"/> that adds a sequence of items.
    /// </summary>
    /// <inheritdoc/>
    public abstract class AddingBlock<T> : BuildingBlock<T>
    {
        /// <summary>
        /// The <see cref="IEnumerable{T}">enumerable</see> sequence that should be added.
        /// </summary>
        protected readonly IEnumerable<T> sequence;

        /// <summary>
        /// Creates a new instance of this building block with the given items.
        /// </summary>
        /// <param name="sequence">A sequence of items that should be added.</param>
        protected AddingBlock(IEnumerable<T> sequence)
        {
            this.sequence = sequence;
        }
    }
}