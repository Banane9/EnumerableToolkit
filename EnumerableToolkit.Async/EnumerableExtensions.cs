#pragma warning disable IDE1006 // Naming Styles

namespace EnumerableToolkit

{
    /// <summary>
    /// Contains handy extension methods for <see cref="IAsyncEnumerable{T}"/>.
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
        /// Outputs all nodes that are part of cycles, given an expression that produces a set of connected nodes.
        /// </summary>
        /// <param name="nodes">The input nodes.</param>
        /// <param name="identifier">Gets the dependency identifier of a node.</param>
        /// <param name="connected">The expression producing connected nodes.</param>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TDependency">The dependency connection type.</typeparam>
        /// <returns>All input nodes that are part of a cycle.</returns>
        public static async IAsyncEnumerable<TNode> FindCycles<TNode, TDependency>(this IAsyncEnumerable<TNode> nodes, Func<TNode, TDependency> identifier, Func<TNode, IEnumerable<TDependency>> connected)

            where TNode : notnull
            where TDependency : notnull
        {
            var currentPath = new Stack<TNode>();
            var visited = new HashSet<TNode>();
            var dependencies = await nodes.ToDictionaryAsync(node => node, node => new HashSet<TDependency>(connected(node)));

            while (dependencies.Count > 0)
            {
                var key = dependencies.FirstOrDefault(x => x.Value.Count == 0).Key
                    ?? throw new ArgumentException($"Cyclic dependencies are not allowed!{Environment.NewLine}" +
                    $"Sorted: {string.Join(", ", nodes.Select(identifier).ToEnumerable().Except(dependencies.Keys.Select(identifier)))}{Environment.NewLine}" +
                    $"Unsorted:{Environment.NewLine}" +
                    $"    {string.Join($"{Environment.NewLine}    ", dependencies.Select(element => $"{identifier(element.Key)}:{Environment.NewLine}        {string.Join($"{Environment.NewLine}        ", element.Value)}"))}");

                dependencies.Remove(key);

                var id = identifier(key);
                foreach (var updateElement in dependencies)
                    updateElement.Value.Remove(id);

                yield return key;
            }
        }

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
        /// Performs a topological sort of the input <paramref name="nodes"/>, given an expression that produces a set of connected nodes.
        /// </summary>
        /// <param name="nodes">The input nodes.</param>
        /// <param name="connected">The expression producing connected nodes.</param>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <returns>The input nodes, sorted.</returns>
        /// <exception cref="ArgumentException">Thrown if a cyclic dependency is found.</exception>
        public static IAsyncEnumerable<TNode> TopologicalSort<TNode>(this IAsyncEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> connected)
            where TNode : notnull => nodes.TopologicalSort(node => node, connected);

        /// <summary>
        /// Performs a topological sort of the input <paramref name="nodes"/>, given an expression that produces a set of connected nodes.
        /// </summary>
        /// <param name="nodes">The input nodes.</param>
        /// <param name="identifier">Gets the dependency identifier of a node.</param>
        /// <param name="connected">The expression producing connected nodes.</param>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TDependency">The dependency connection type.</typeparam>
        /// <returns>The input nodes, sorted .</returns>
        /// <exception cref="ArgumentException">Thrown if a cyclic dependency is found.</exception>
        public static async IAsyncEnumerable<TNode> TopologicalSort<TNode, TDependency>(this IAsyncEnumerable<TNode> nodes, Func<TNode, TDependency> identifier, Func<TNode, IEnumerable<TDependency>> connected)
            where TNode : notnull
            where TDependency : notnull
        {
            var elements = await nodes.ToDictionaryAsync(node => node, node => new HashSet<TDependency>(connected(node)));

            while (elements.Count > 0)
            {
                var key = elements.FirstOrDefault(x => x.Value.Count == 0).Key
                    ?? throw new ArgumentException($"Cyclic dependencies are not allowed!{Environment.NewLine}" +
                    $"Sorted: {string.Join(", ", nodes.Select(identifier).ToEnumerable().Except(elements.Keys.Select(identifier)))}{Environment.NewLine}" +
                    $"Unsorted:{Environment.NewLine}" +
                    $"    {string.Join($"{Environment.NewLine}    ", elements.Select(element => $"{identifier(element.Key)}:{Environment.NewLine}        {string.Join($"{Environment.NewLine}        ", element.Value)}"))}");

                elements.Remove(key);

                var id = identifier(key);
                foreach (var updateElement in elements)
                    updateElement.Value.Remove(id);

                yield return key;
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