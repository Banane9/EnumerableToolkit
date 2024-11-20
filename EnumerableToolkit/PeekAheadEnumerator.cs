using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit
{
    /// <summary>
    /// Wraps an <see cref="IEnumerator{T}"/> to only be evaluated once
    /// across different branches, unless it is <see cref="Reset">reset</see>.
    /// </summary>
    /// <remarks>
    /// This enumerator is intended for searches in a sequence
    /// that make use of branching and backtracking.
    /// As such, <see cref="MoveToCurrentPeekPosition"/> <see cref="Reset">resetting</see> an instance of it
    /// will break any related instances.<br/>
    /// <br/>
    /// This class is thread safe.
    /// </remarks>
    /// <typeparam name="T">The type of items in the sequence.</typeparam>
    public sealed class PeekAheadEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly List<T> _peekedElements = [default!];
        private int _currentIndex = 0;

        /// <remarks>
        /// This refers to the current peek position
        /// that the enumerator was advanced to.
        /// </remarks>
        /// <inheritdoc/>
        public T Current => _peekedElements[_currentIndex];

        object IEnumerator.Current => Current!;

        /// <summary>
        /// Gets the index of the currently last peeked item of
        /// the wrapped enumerator in <see cref="_peekedElements"/>.
        /// </summary>
        private int CurrentMaxPeekIndex => _peekedElements.Count - 1;

        /// <summary>
        /// Wraps the given <paramref name="enumerator"/> to only evaluate it once.
        /// </summary>
        /// <param name="enumerator">The enumerator to wrap.</param>
        public PeekAheadEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        /// <summary>
        /// Wraps the <see cref="IEnumerable{T}.GetEnumerator">enumerator</see>
        /// of the given <paramref name="enumerable"/> to only evaluate it once.
        /// </summary>
        /// <param name="enumerable">The enumerable to enumerate.</param>
        public PeekAheadEnumerator(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator())
        { }

        private PeekAheadEnumerator(PeekAheadEnumerator<T> template)
        {
            _enumerator = template._enumerator;
            _peekedElements = template._peekedElements;
            _currentIndex = template._currentIndex;
        }

        /// <remarks>
        /// Disposes the wrapped enumerator.<br/>
        /// This affects all instances related to this.
        /// </remarks>
        /// <inheritdoc/>
        public void Dispose() => _enumerator.Dispose();

        /// <remarks>
        /// If the wrapped enumerator was already advanced past this point,
        /// the already enumerated items will be available as <see cref="Current">Current</see>.<br/>
        /// Otherwise, the wrapped enumerator is advanced and its
        /// new <see cref="IEnumerator{T}.Current">Current</see> item is recorded.
        /// </remarks>
        /// <inheritdoc/>
        public bool MoveNext()
        {
            lock (_peekedElements)
            {
                if (_currentIndex == CurrentMaxPeekIndex)
                {
                    ++_currentIndex;

                    if (_enumerator.MoveNext())
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
        }

        /// <summary>
        /// Resets this enumerator's peek position
        /// and advances its true starting point by one.
        /// </summary>
        /// <remarks>
        /// This affects all instances related to this.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the enumerator was successfully advanced to the next element;
        /// <c>false</c> if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNextAndResetPeek()
        {
            lock (_peekedElements)
            {
                ResetPeek();

                if (CurrentMaxPeekIndex == 0)
                {
                    if (_enumerator.MoveNext())
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

        /// <remarks>
        /// This affects all instances related to this.
        /// </remarks>
        /// <inheritdoc/>
        public void Reset()
        {
            lock (_peekedElements)
            {
                ResetPeek();
                _enumerator.Reset();
                _peekedElements.Clear();
                _peekedElements.Add(default!);
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
        public PeekAheadEnumerator<T> Snapshot() => new(this);
    }
}
