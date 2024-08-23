namespace EnumerableToolkit
{
    /// <summary>
    /// Represents a collection which is kept sorted using the items' <see cref="IPrioritizable.Priority"/>
    /// using the <see cref="Prioritizable.Comparer"/> (higher sorts first).
    /// </summary>
    /// <remarks>
    /// Duplicate items, and multiple items comparing equal are supported.
    /// </remarks>
    /// <inheritdoc/>
    public class PrioritySortedCollection<T> : SortedCollection<T>
        where T : notnull, IPrioritizable
    {
        /// <summary>
        /// Constructs an empty sorted collection using the items' <see cref="IPrioritizable.Priority"/> for sorting.
        /// </summary>
        public PrioritySortedCollection()
            : base((IComparer<T>)Prioritizable.Comparer)
        { }

        /// <summary>
        /// Constructs a sorted collection with the given elements,
        /// while using the items' <see cref="IPrioritizable.Priority"/> for sorting.
        /// </summary>
        /// <param name="collection">The elements to add to the collection.</param>
        public PrioritySortedCollection(IEnumerable<T>? collection)
            : base(collection, (IComparer<T>)Prioritizable.Comparer)
        { }
    }
}