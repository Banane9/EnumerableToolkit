namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods related to the <see cref="PeekAheadAsyncEnumerator{T}"/>.
    /// </summary>
    public static class PeekAheadAsyncEnumeratorExtensions
    {
        /// <summary>
        /// Returns a <see cref="PeekAheadAsyncEnumerator{T}"/> that
        /// iterates through the collection once across different branches.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="asyncEnumerable">The async enumerable to iterate.</param>
        /// <returns>A <see cref="PeekAheadAsyncEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public static PeekAheadAsyncEnumerator<T> GetPeekAheadEnumerator<T>(this IAsyncEnumerable<T> asyncEnumerable)
            => new(asyncEnumerable);

        /// <summary>
        /// Returns a <see cref="PeekAheadAsyncEnumerator{T}"/> that wraps the given
        /// <paramref name="asyncEnumerator"/> and iterates through the collection once across different branches.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="asyncEnumerator">The enumerator to wrap.</param>
        /// <returns>A <see cref="PeekAheadAsyncEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public static PeekAheadAsyncEnumerator<T> MakePeekAhead<T>(this IAsyncEnumerator<T> asyncEnumerator)
            => new(asyncEnumerator);
    }
}
