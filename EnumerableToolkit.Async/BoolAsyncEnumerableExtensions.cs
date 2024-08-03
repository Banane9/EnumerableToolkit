namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IAsyncEnumerable{T}">async enumerable</see> sequences of <see cref="bool">booleans</see>.
    /// </summary>
    public static class BoolAsyncEnumerableExtensions
    {
        /// <summary>
        /// Determines whether all elements of a boolean sequence are <c>false</c>.
        /// </summary>
        /// <param name="source">The source sequence to check.</param>
        /// <returns>
        /// <c>true</c> if every element of the source sequence is <c>false</c>,
        /// or if the sequence is empty; otherwise, <c>false</c>.
        /// </returns>
        public static async Task<bool> AllFalseAsync(this IAsyncEnumerable<bool> source)
            => !await source.AnyTrueAsync();

        /// <summary>
        /// Determines whether all elements of a boolean sequence are <c>true</c>.
        /// </summary>
        /// <param name="source">The source sequence to check.</param>
        /// <returns>
        /// <c>true</c> if every element of the source sequence is <c>true</c>,
        /// or if the sequence is empty; otherwise, <c>false</c>.
        /// </returns>
        public static async Task<bool> AllTrueAsync(this IAsyncEnumerable<bool> source)
            => !await source.AnyFalseAsync();

        /// <summary>
        /// Determines whether any element of a boolean sequence is <c>false</c>.
        /// </summary>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if any element of the source sequence is <c>false</c>; otherwise, <c>true</c>.</returns>
        public static async Task<bool> AnyFalseAsync(this IAsyncEnumerable<bool> source)
        {
            await foreach (var item in source)
            {
                if (!item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether any element of a boolean sequence is <c>true</c>.
        /// </summary>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if any element of the source sequence is <c>true</c>; otherwise, <c>false</c>.</returns>
        public static async Task<bool> AnyTrueAsync(this IAsyncEnumerable<bool> source)
        {
            await foreach (var item in source)
            {
                if (item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether an even number of elements of a boolean sequence is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to aggregating over the negated sequence with a xor and negating the result.
        /// </remarks>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if an even number of elements of the source sequence is <c>false</c>; otherwise, <c>false</c>.</returns>
        public static async Task<bool> EvenFalseAsync(this IAsyncEnumerable<bool> source)
            => !await UnevenFalseAsync(source);

        /// <summary>
        /// Determines whether an even number of elements of a boolean sequence is <c>true</c>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to aggregating over the sequence with a xor and negating the result.
        /// </remarks>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if an even number of elements of the source sequence is <c>true</c>; otherwise, <c>false</c>.</returns>
        public static async Task<bool> EvenTrueAsync(this IAsyncEnumerable<bool> source)
            => !await UnevenTrueAsync(source);

        /// <summary>
        /// Determines whether an uneven number of elements of a boolean sequence is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to aggregating over the negated sequence with a xor.
        /// </remarks>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if an uneven number of elements of the source sequence is <c>false</c>; otherwise, <c>false</c>.</returns>
        public static async Task<bool> UnevenFalseAsync(this IAsyncEnumerable<bool> source)
            => await source.AggregateAsync((acc, item) => acc ^ !item);

        /// <summary>
        /// Determines whether an uneven number of elements of a boolean sequence is <c>true</c>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to aggregating over the sequence with a xor.
        /// </remarks>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if an uneven number of elements of the source sequence is <c>true</c>; otherwise, <c>false</c>.</returns>
        public static async Task<bool> UnevenTrueAsync(this IAsyncEnumerable<bool> source)
            => await source.AggregateAsync((acc, item) => acc ^ item);
    }
}