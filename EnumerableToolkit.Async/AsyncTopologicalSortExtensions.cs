#pragma warning disable IDE1006 // Naming Styles

namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IAsyncEnumerable{T}">async enumerable</see> sequences that allow topological sorting.
    /// </summary>
    public static class AsyncTopologicalSortExtensions
    {
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
    }
}

#pragma warning restore IDE1006 // Naming Styles