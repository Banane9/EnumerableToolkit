namespace EnumerableToolkit
{
    /// <summary>
    /// Contains a comparer for <see cref="IPrioritizable">prioritizable</see> things
    /// and some other helpful extension methods.
    /// </summary>
    public static class Prioritizable
    {
        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> that sorts <see cref="IPrioritizable"/> instances
        /// by their <see cref="IPrioritizable.Priority">Priority</see> in descending order.
        /// </summary>
        public static IComparer<IPrioritizable> Comparer { get; } = new PrioritizableComparer();

        /// <summary>
        /// Unwraps the given sequence of <see cref="IPrioritizable">prioritizable</see>
        /// items to their <see cref="Prioritizable{T}.Value">values</see>.
        /// </summary>
        /// <typeparam name="T">The type of the item to make prioritizable.</typeparam>
        /// <param name="prioritizableSequence">The sequence of prioritizable items to unwrap.</param>
        /// <returns>A sequence of the prioritizable item's values.</returns>
        public static IEnumerable<T> Unwrap<T>(this IEnumerable<Prioritizable<T>> prioritizableSequence)
            => prioritizableSequence.Select(prioritizable => prioritizable.Value);

        /// <summary>
        /// Wraps the given value to make it <see cref="IPrioritizable">prioritizable</see>
        /// with the given <paramref name="priority"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to make prioritizable.</typeparam>
        /// <param name="value">The value to wrap and make prioritizable.</param>
        /// <param name="priority">The priority to assign to the <paramref name="value"/>.</param>
        public static Prioritizable<T> WithPriority<T>(this T value, int priority)
            => new(value, priority);

        /// <summary>
        /// Wraps the given values to make a <see cref="IPrioritizable">prioritizable</see>
        /// sequence with the given <paramref name="priority"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the item to make prioritizable.</typeparam>
        /// <param name="sequence">The values to wrap and make prioritizable.</param>
        /// <param name="priority">The priority to assign to the values of the <paramref name="sequence"/>.</param>
        public static IEnumerable<Prioritizable<T>> WithPriority<T>(this IEnumerable<T> sequence, int priority)
            => sequence.Select(sequence => sequence.WithPriority(priority));

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

    /// <summary>
    /// Wraps any item to make it <see cref="IPrioritizable">prioritizable</see>.
    /// </summary>
    /// <typeparam name="T">The type of the item to make prioritizable.</typeparam>
    public sealed class Prioritizable<T> : IPrioritizable
    {
        /// <inheritdoc/>
        public int Priority { get; }

        /// <summary>
        /// Gets the wrapped value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Wraps the given value to make it <see cref="IPrioritizable">prioritizable</see>
        /// with the given <paramref name="priority"/>.
        /// </summary>
        /// <param name="value">The value to wrap and make prioritizable.</param>
        /// <param name="priority">The priority to assign to the <paramref name="value"/>.</param>
        public Prioritizable(T value, int priority = 0)
        {
            Value = value;
            Priority = priority;
        }

        /// <summary>
        /// Wraps the given value to make it <see cref="IPrioritizable">prioritizable</see>.
        /// </summary>
        /// <param name="value">The value to wrap and make prioritizable.</param>
        public static implicit operator Prioritizable<T>(T value)
            => new(value);

        /// <summary>
        /// Wraps the given tuple's value and priority together to make a <see cref="IPrioritizable">prioritizable</see> value.
        /// </summary>
        /// <param name="prioritizedValue">The tuple containing the value and priority to wrap together.</param>
        public static implicit operator Prioritizable<T>((T Value, int Priority) prioritizedValue)
            => new(prioritizedValue.Value, prioritizedValue.Priority);

        /// <summary>
        /// Unwraps the <see cref="IPrioritizable">prioritizable</see> <see cref="Prioritizable{T}.Value">value</see>.
        /// </summary>
        /// <param name="prioritizableValue">The wrapped prioritizable value to unwrap.</param>
        public static implicit operator T(Prioritizable<T> prioritizableValue)
            => prioritizableValue.Value;
    }
}