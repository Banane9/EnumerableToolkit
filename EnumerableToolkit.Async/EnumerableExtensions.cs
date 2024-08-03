#pragma warning disable IDE1006 // Naming Styles

namespace EnumerableToolkit

{
    /// <summary>
    /// Contains general extension methods for <see cref="IAsyncEnumerable{T}">async enumerable</see> sequences.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates a single item to a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>The sequence with the concatenated item.</returns>
        public static async IAsyncEnumerable<T> Concat<T>(this IAsyncEnumerable<T> source, T item)
        {
            await foreach (var sourceItem in source)
                yield return sourceItem;

            yield return item;
        }

        /// <summary>
        /// Concatenates a synchronous sequence to the async enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <param name="sequence">The sequence to concat.</param>
        /// <returns>The sequence with the concatenated sequence.</returns>
        public static IAsyncEnumerable<T> Concat<T>(this IAsyncEnumerable<T> source, IEnumerable<T> sequence)
            => source.Concat(sequence.ToAsyncEnumerable());

        /// <summary>
        /// Tries to cast every item from the <paramref name="source"/> to <typeparamref name="TTo"/>.
        /// </summary>
        /// <typeparam name="TFrom">The items in the source sequence.</typeparam>
        /// <typeparam name="TTo">The items in the result sequence.</typeparam>
        /// <param name="source">The items to try and cast.</param>
        /// <returns>All items from the source that were castable to <typeparamref name="TTo"/> and not <c>null</c>.</returns>
        public static async IAsyncEnumerable<TTo> SelectWhereCastable<TFrom, TTo>(this IAsyncEnumerable<TFrom?> source)
        {
            await foreach (var item in source)
            {
                if (item is TTo toItem)
                    yield return toItem;
            }
        }

        /// <summary>
        /// Tries to transform each item in the <paramref name="source"/> sequence using the <paramref name="trySelector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of items in the result sequence.</typeparam>
        /// <param name="source">The source sequence to transform.</param>
        /// <param name="trySelector">A selector following the try-pattern.</param>
        /// <returns>A result sequence containing only the successfully transformed items.</returns>
        public static async IAsyncEnumerable<TResult> TrySelect<TSource, TResult>(this IAsyncEnumerable<TSource> source, TryConverter<TSource, TResult> trySelector)
        {
            await foreach (var item in source)
            {
                if (trySelector(item, out var result))
                    yield return result;
            }
        }
    }
}

#pragma warning restore IDE1006 // Naming Styles