namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for the individual items that might be used in <see cref="IEnumerable{T}">enumerable</see> sequences.
    /// </summary>
    public static class EnumerableItemExtensions
    {
        /// <summary>
        /// Returns the given item without doing anything.
        /// </summary>
        /// <typeparam name="T">The type of the item in the sequence.</typeparam>
        /// <param name="item">The item to return.</param>
        /// <returns>The given item.</returns>
        public static T Identity<T>(T item) => item;

        /// <summary>
        /// Turns a single item into a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="item">The item to make into a sequence.</param>
        /// <returns>The item as a sequence.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}