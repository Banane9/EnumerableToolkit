namespace EnumerableToolkit
{
    /// <summary>
    /// Contains a comparer for <see cref="IPrioritizable">prioritizable</see> things.
    /// </summary>
    public static class Prioritizable
    {
        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> that sorts <see cref="IPrioritizable"/> instances
        /// by their <see cref="IPrioritizable.Priority">Priority</see> in descending order.
        /// </summary>
        public static IComparer<IPrioritizable> Comparer { get; } = new PrioritizableComparer();

        private sealed class PrioritizableComparer : IComparer<IPrioritizable>
        {
            public int Compare(IPrioritizable x, IPrioritizable y)
                => Comparer<int>.Default.Compare(y.Priority, x.Priority);
        }
    }

    /// <summary>
    /// Defines the interface for prioritizable things.
    /// </summary>
    public interface IPrioritizable
    {
        /// <summary>
        /// Gets the priority of this item. Use an agreed standard as a base.
        /// </summary>
        /// <value>
        /// An interger used to sort the prioritizable items.<br/>
        /// Higher comes first with the <see cref="Prioritizable.Comparer">default comparer</see>.
        /// </value>
        public int Priority { get; }
    }
}