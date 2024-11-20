namespace EnumerableToolkit
{
    /// <summary>
    /// Wraps an <see cref="IAsyncEnumerator{T}"/> to only be evaluated once across different branches.
    /// </summary>
    /// <remarks>
    /// This enumerator is intended for searches in a sequence
    /// that make use of branching and backtracking.
    /// As such, <see cref="MoveToCurrentPeekPosition">moving to the current peek position</see>
    /// on an instance of it will break any related instances.<br/>
    /// <br/>
    /// This class is thread safe.
    /// </remarks>
    /// <typeparam name="T">The type of items in the sequence.</typeparam>
    public sealed class PeekAheadAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IAsyncEnumerator<T> _enumerator;
        private readonly List<T> _peekedElements = [default!];
        private int _currentIndex = 0;

        /// <remarks>
        /// This refers to the current peek position
        /// that the async enumerator was advanced to.
        /// </remarks>
        /// <inheritdoc/>
        public T Current => _peekedElements[_currentIndex];

        /// <summary>
        /// Gets the index of the currently last peeked item of
        /// the wrapped async enumerator in <see cref="_peekedElements"/>.
        /// </summary>
        private int CurrentMaxPeekIndex => _peekedElements.Count - 1;

        /// <summary>
        /// Wraps the given <paramref name="asyncEnumerator"/> to only evaluate it once.
        /// </summary>
        /// <param name="asyncEnumerator">The async enumerator to wrap.</param>
        public PeekAheadAsyncEnumerator(IAsyncEnumerator<T> asyncEnumerator)
        {
            _enumerator = asyncEnumerator;
        }

        /// <summary>
        /// Wraps the <see cref="IAsyncEnumerable{T}.GetAsyncEnumerator">enumerator</see>
        /// of the given <paramref name="asyncEnumerable"/> to only evaluate it once.
        /// </summary>
        /// <param name="asyncEnumerable">The enumerable to iterate.</param>
        public PeekAheadAsyncEnumerator(IAsyncEnumerable<T> asyncEnumerable)
            : this(asyncEnumerable.GetAsyncEnumerator())
        { }

        private PeekAheadAsyncEnumerator(PeekAheadAsyncEnumerator<T> template)
        {
            _enumerator = template._enumerator;
            _peekedElements = template._peekedElements;
            _currentIndex = template._currentIndex;
        }

        /// <remarks>
        /// Disposes the wrapped async enumerator.<br/>
        /// This affects all instances related to this.
        /// </remarks>
        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
            => await _enumerator.DisposeAsync().ConfigureAwait(true);

        /// <summary>
        /// Asynchronously resets this enumerator's peek position
        /// and advances its true starting point by one.
        /// </summary>
        /// <remarks>
        /// This affects all instances related to this.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the enumerator was successfully advanced to the next element;
        /// <c>false</c> if the enumerator has passed the end of the collection.
        /// </returns>
        public async ValueTask<bool> MoveNextAndResetPeekAsync()
        {
            Monitor.Enter(_peekedElements);

            try
            {
                ResetPeek();

                if (CurrentMaxPeekIndex == 0)
                {
                    if (await _enumerator.MoveNextAsync().ConfigureAwait(true))
                    {
                        _peekedElements[0] = _enumerator.Current;
                        return true;
                    }

                    _peekedElements[0] = default!;
                    return false;
                }

                // When peekedElements.Count > 1
                _peekedElements.RemoveAt(0);
                return true;
            }
            finally
            {
                Monitor.Exit(_peekedElements);
            }
        }

        /// <remarks>
        /// If the wrapped enumerator was already advanced past this point,
        /// the already enumerated items will be available as <see cref="Current">Current</see>.<br/>
        /// Otherwise, the wrapped enumerator is advanced and its
        /// new <see cref="IAsyncEnumerator{T}.Current">Current</see> item is recorded.
        /// </remarks>
        /// <inheritdoc/>
        public async ValueTask<bool> MoveNextAsync()
        {
            Monitor.Enter(_peekedElements);

            try
            {
                if (_currentIndex == CurrentMaxPeekIndex)
                {
                    ++_currentIndex;

                    if (await _enumerator.MoveNextAsync().ConfigureAwait(true))
                    {
                        _peekedElements.Add(_enumerator.Current);
                        return true;
                    }

                    _peekedElements.Add(default!);
                    return false;
                }

                // When currentIndex before the end of peekedElements
                ++_currentIndex;
                return true;
            }
            finally
            {
                Monitor.Exit(_peekedElements);
            }
        }

        /// <summary>
        /// Advances this enumerator's true starting point to the current peek position.
        /// </summary>
        /// <remarks>
        /// This affects all instances related to this.
        /// </remarks>
        /// <returns><c>true</c></returns>
        public bool MoveToCurrentPeekPosition()
        {
            lock (_peekedElements)
            {
                _peekedElements.RemoveRange(0, _currentIndex);
                ResetPeek();
                return true;
            }
        }

        /// <summary>
        /// Resets this enumerator's peek position to its true starting point.
        /// </summary>
        public void ResetPeek() => _currentIndex = 0;

        /// <summary>
        /// Creates a related instance from this enumerator that can be advanced individually
        /// while only evaluating the wrapped enumerator once.
        /// </summary>
        /// <returns>The new related instance wrapping the same enumerator.</returns>
        public PeekAheadAsyncEnumerator<T> Snapshot() => new(this);
    }
}