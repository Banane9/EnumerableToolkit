namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IAsyncEnumerable{T}">enumerable</see> sequences of <see cref="Delegate">delegates</see>.
    /// </summary>
    public static class DelegateAsyncEnumerableExtensions
    {
        /// <summary>
        /// Sequentially wraps every given <see cref="Delegate"/> into a <see cref="Task"/>, calling and awaiting them
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="delegates">The <see cref="Task"/>-returning delegates to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static async Task TryInvokeAllAsync(this IAsyncEnumerable<Delegate>? delegates, params object[] args)
        {
            if (delegates is null || !await delegates.AnyAsync())
                return;

            var exceptions = new List<Exception>();

            await foreach (var handler in delegates)
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
        /// Sequentially calls and awaits every given <see cref="Delegate"/>
        /// in a try-catch-block, collecting any <see cref="Exception"/>s into an <see cref="AggregateException"/>.<br/>
        /// The delegates must return a <see cref="Task"/>.
        /// </summary>
        /// <param name="delegates">The <see cref="Task"/>-returning delegates to safely invoke.</param>
        /// <param name="args">The arguments for the invocation.</param>
        /// <exception cref="AggregateException">Thrown when any invoked methods threw. Contains all nested Exceptions.</exception>
        public static async Task TryInvokeAllTasksAsync(this IAsyncEnumerable<Delegate>? delegates, params object[] args)
        {
            if (delegates is null || !await delegates.AnyAsync())
                return;

            var exceptions = new List<Exception>();

            await foreach (var handler in delegates!)
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