using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EnumerableToolkit
{
    /// <summary>
    /// Represents a method that converts an object from one type to another type following the try-pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of object that is to be converted.</typeparam>
    /// <typeparam name="TOutput">The type the input object is to be converted to.</typeparam>
    /// <param name="input">The object to convert.</param>
    /// <param name="output">The result item when successful, or <c>null</c> otherwise.</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
    public delegate bool TryConverter<in TInput, TOutput>(TInput input, [NotNullWhen(true)] out TOutput? output);

    /// <summary>
    /// Contains general extension methods for <see cref="IEnumerable{T}">enumerable</see> sequences.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Wraps the given <see cref="IEnumerable{T}"/> in an iterator to prevent casting it to its underlaying type.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="enumerable">The enumerable to wrap.</param>
        /// <returns>An iterator wrapping the input enumerable.</returns>
        public static IEnumerable<T> AsSafeEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                yield return item;
        }

        /// <summary>
        /// Concatenates a single item to a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>The sequence with the concatenated item.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            foreach (var sourceItem in source)
                yield return sourceItem;

            yield return item;
        }

        /// <summary>
        /// Filters a source sequence of <see cref="Type"/>s to only contain the ones instantiable
        /// without parameters and assignable to <typeparamref name="TInstance"/>.
        /// </summary>
        /// <typeparam name="TInstance">The type that sequence items must be assignable to.</typeparam>
        /// <param name="types">The source sequence of types to filter.</param>
        /// <returns>A sequence of instantiable types assignable to <typeparamref name="TInstance"/>.</returns>
        public static IEnumerable<Type> ParameterlessInstantiable<TInstance>(this IEnumerable<Type> types)
        {
            var instanceType = typeof(TInstance);

            foreach (var type in types)
            {
                if (!type.IsAbstract && !type.ContainsGenericParameters && instanceType.IsAssignableFrom(type)
                    && (type.IsValueType || type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) is not null))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Tries to cast every item from the <paramref name="source"/> to <typeparamref name="TTo"/>.
        /// </summary>
        /// <typeparam name="TFrom">The items in the source sequence.</typeparam>
        /// <typeparam name="TTo">The items in the result sequence.</typeparam>
        /// <param name="source">The items to try and cast.</param>
        /// <returns>All items from the source that were castable to <typeparamref name="TTo"/> and not <c>null</c>.</returns>
        public static IEnumerable<TTo> SelectWhereCastable<TFrom, TTo>(this IEnumerable<TFrom?> source)
        {
            foreach (var item in source)
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
        public static IEnumerable<TResult> TrySelect<TSource, TResult>(this IEnumerable<TSource> source, TryConverter<TSource, TResult> trySelector)
        {
            foreach (var item in source)
            {
                if (trySelector(item, out var result))
                    yield return result;
            }
        }
    }
}