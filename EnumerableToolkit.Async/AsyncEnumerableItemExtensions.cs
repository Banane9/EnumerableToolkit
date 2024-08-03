namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for the individual items that might be used in <see cref="IAsyncEnumerable{T}">async enumerable</see> sequences.
    /// </summary>
    public static class AsyncEnumerableItemExtensions
    {
        /// <summary>
        /// Turns a single item into an async sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="item">The item to make into a sequence.</param>
        /// <returns>The item as an async sequence.</returns>

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public static async IAsyncEnumerable<T> YieldAsync<T>(this T item)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            yield return item;
        }
    }
}