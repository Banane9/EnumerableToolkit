namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}">enumerable</see> sequences of <see cref="Delegate">delegates</see>.
    /// </summary>
    public static class DelegateEnumerableExtensions
    {
        /// <summary>
        /// Individually calls every method from the given <see cref="Delegate"/>'s invocation list in a try-catch-block,
        /// collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="del">The delegate to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static void TryInvokeAll(this Delegate? del, params object[] args)
            => del?.GetInvocationList().TryInvokeAll(args);

        /// <summary>
        /// Individually calls all given <paramref name="delegates"/> in a try-catch-block,
        /// collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="delegates">The delegates to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static void TryInvokeAll(this IEnumerable<Delegate>? delegates, params object[] args)
        {
            if (delegates is null || !delegates.Any())
                return;

            var exceptions = new List<Exception>();

            foreach (var handler in delegates!)
            {
                try
                {
                    if (handler.GetInvocationList().Length > 1)
                        handler.GetInvocationList().TryInvokeAll(args);
                    else
                        handler.Method.Invoke(handler.Target, args);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Sequentially calls and awaits every async method from the given <see cref="Delegate"/>'s invocation list
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// The delegate must return a <see cref="Task"/>.
        /// </summary>
        /// <summary>
        /// Sequentially wraps every method from the given <see cref="Delegate"/>'s
        /// invocation list into a <see cref="Task"/>, calling and awaiting them
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="del">The <see cref="Task"/>-returning delegate to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static Task TryInvokeAllAsync(this Delegate? del, params object[] args)
            => del?.GetInvocationList().TryInvokeAllAsync(args) ?? Task.CompletedTask;

        /// <summary>
        /// Sequentially wraps every given <see cref="Delegate"/> into a <see cref="Task"/>, calling and awaiting them
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="delegates">The <see cref="Task"/>-returning delegates to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static async Task TryInvokeAllAsync(this IEnumerable<Delegate>? delegates, params object[] args)
        {
            if (delegates is null || !delegates.Any())
                return;

            var exceptions = new List<Exception>();

            foreach (var handler in delegates)
            {
                try
                {
                    if (handler.GetInvocationList().Length > 1)
                        await handler.GetInvocationList().TryInvokeAllAsync(args);
                    else
                        await Task.Run(() => handler.Method.Invoke(handler.Target, args));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Sequentially calls and awaits every async method from the given <see cref="Delegate"/>'s invocation list
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.<br/>
        /// The delegate must return a <see cref="Task"/>.
        /// </summary>
        /// <param name="del">The <see cref="Task"/>-returning delegate to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static Task TryInvokeAllTasksAsync(this Delegate? del, params object[] args)
            => del?.GetInvocationList().TryInvokeAllTasksAsync(args) ?? Task.CompletedTask;

        /// <summary>
        /// Sequentially calls and awaits every given <see cref="Delegate"/>
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.<br/>
        /// The delegates must return a <see cref="Task"/>.
        /// </summary>
        /// <param name="delegates">The <see cref="Task"/>-returning delegates to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static async Task TryInvokeAllTasksAsync(this IEnumerable<Delegate>? delegates, params object[] args)
        {
            if (delegates is null || !delegates.Any())
                return;

            var exceptions = new List<Exception>();

            foreach (var handler in delegates)
            {
                try
                {
                    if (handler.GetInvocationList().Length > 1)
                        await handler.GetInvocationList().TryInvokeAllTasksAsync(args);
                    else
                        await (Task)handler.Method.Invoke(handler.Target, args);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}