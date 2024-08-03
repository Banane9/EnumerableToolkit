namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}">enumerable</see> sequences that allow topological sorting.
    /// </summary>
    public static class TopologicalSortExtensions
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
        public static IEnumerable<TNode> FindCycles<TNode, TDependency>(this IEnumerable<TNode> nodes, Func<TNode, TDependency> identifier, Func<TNode, IEnumerable<TDependency>> connected)
            where TNode : notnull
            where TDependency : notnull
        {
            var currentPath = new Stack<TNode>();
            var visited = new HashSet<TNode>();
            var dependencies = nodes.ToDictionary(node => node, node => new HashSet<TDependency>(connected(node)));

            while (dependencies.Count > 0)
            {
                var key = dependencies.FirstOrDefault(x => x.Value.Count == 0).Key
                    ?? throw new ArgumentException($"Cyclic dependencies are not allowed!{Environment.NewLine}" +
                    $"Sorted: {string.Join(", ", nodes.Select(identifier).Except(dependencies.Keys.Select(identifier)))}{Environment.NewLine}" +
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
        /// <param name="identifier">Gets the dependency identifier of a node.</param>
        /// <param name="connected">The expression producing connected nodes.</param>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TDependency">The dependency connection type.</typeparam>
        /// <returns>The input nodes, sorted .</returns>
        /// <exception cref="ArgumentException">Thrown if a cyclic dependency is found.</exception>
        public static IEnumerable<TNode> TopologicalSort<TNode, TDependency>(this IEnumerable<TNode> nodes, Func<TNode, TDependency> identifier, Func<TNode, IEnumerable<TDependency>> connected)
            where TNode : notnull
            where TDependency : notnull
        {
            var elements = nodes.ToDictionary(node => node, node => new HashSet<TDependency>(connected(node)));

            while (elements.Count > 0)
            {
                var key = elements.FirstOrDefault(x => x.Value.Count == 0).Key
                    ?? throw new ArgumentException($"Cyclic dependencies are not allowed!{Environment.NewLine}" +
                    $"Sorted: {string.Join(", ", nodes.Select(identifier).Except(elements.Keys.Select(identifier)))}{Environment.NewLine}" +
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
        /// Performs a topological sort of the input <paramref name="nodes"/>, given an expression that produces a set of connected nodes.
        /// </summary>
        /// <param name="nodes">The input nodes.</param>
        /// <param name="connected">The expression producing connected nodes.</param>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <returns>The input nodes, sorted.</returns>
        /// <exception cref="ArgumentException">Thrown if a cyclic dependency is found.</exception>
        public static IEnumerable<TNode> TopologicalSort<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> connected)
            where TNode : notnull => nodes.TopologicalSort(node => node, connected);
    }
}