namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods related to the <see cref="PeekAheadEnumerator{T}"/>.
    /// </summary>
    public static class PeekAheadEnumeratorExtensions
    {
        /// <summary>
        /// Returns a <see cref="PeekAheadEnumerator{T}"/> that iterates through the collection
        /// once across different branches, unless it is <see cref="PeekAheadEnumerator{T}.Reset">reset</see>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="enumerable">The enumerable to iterate.</param>
        /// <returns>A <see cref="PeekAheadEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public static PeekAheadEnumerator<T> GetPeekAheadEnumerator<T>(this IEnumerable<T> enumerable)
            => new(enumerable);

        /// <summary>
        /// Returns a <see cref="PeekAheadEnumerator{T}"/> that wraps
        /// the given <paramref name="enumerator"/> and iterates through the collection
        /// once across different branches, unless it is <see cref="PeekAheadEnumerator{T}.Reset">reset</see>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="enumerator">The enumerator to wrap.</param>
        /// <returns>A <see cref="PeekAheadEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public static PeekAheadEnumerator<T> MakePeekAhead<T>(this IEnumerator<T> enumerator)
            => new(enumerator);
    }
}