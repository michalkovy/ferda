// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Ferda.Modules.Helpers.Caching
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to all of the
        /// sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// Buffer enabling each enumerator to retrieve all elements from the shared source sequence, without duplicating
        /// source enumeration side-effects.
        /// </returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Do(x => Console.WriteLine(x)).Memoize();
        /// var e1 = rng.GetAsyncEnumerator();
        /// Assert.IsTrue(e1.MoveNext());    // Prints 0
        /// Assert.AreEqual(0, e1.Current);
        /// Assert.IsTrue(e1.MoveNext());    // Prints 1
        /// Assert.AreEqual(1, e1.Current);
        /// var e2 = rng.GetAsyncEnumerator();
        /// Assert.IsTrue(e2.MoveNext());    // Doesn't print anything; the side-effect of Do
        /// Assert.AreEqual(0, e2.Current);  // has already taken place during e1's iteration.
        /// Assert.IsTrue(e1.MoveNext());    // Prints 2
        /// Assert.AreEqual(2, e1.Current);
        /// </example>
        public static IBuffer<TSource> Memoize<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MemoizedBuffer<TSource>(source.GetAsyncEnumerator());
        }

        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing a specified number of enumerators to obtain access
        /// to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">
        /// Number of enumerators that can access the underlying buffer. Once every enumerator has
        /// obtained an element from the buffer, the element is removed from the buffer.
        /// </param>
        /// <returns>
        /// Buffer enabling a specified number of enumerators to retrieve all elements from the shared source sequence,
        /// without duplicating source enumeration side-effects.
        /// </returns>
        public static IBuffer<TSource> Memoize<TSource>(this IAsyncEnumerable<TSource> source, int readerCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (readerCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(readerCount));

            return new MemoizedBuffer<TSource>(source.GetAsyncEnumerator(), readerCount);
        }

        private sealed class MemoizedBuffer<T> : IBuffer<T>
        {
            private readonly AsyncLock _gate = new AsyncLock();
            private readonly IRefCountList<T> _buffer;
            private readonly IAsyncEnumerator<T> _source;

            private bool _disposed;
            private Exception _error;
            private bool _stopped;

            public MemoizedBuffer(IAsyncEnumerator<T> source)
                : this(source, new MaxRefCountList<T>())
            {
            }

            public MemoizedBuffer(IAsyncEnumerator<T> source, int readerCount)
                : this(source, new RefCountList<T>(readerCount))
            {
            }

            private MemoizedBuffer(IAsyncEnumerator<T> source, IRefCountList<T> buffer)
            {
                _source = source;
                _buffer = buffer;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetAsyncEnumerator_();
            }

            public async ValueTask DisposeAsync()
            {
                using (await _gate.LockAsync().ConfigureAwait(false))
                {
                    if (!_disposed)
                    {
                        await _source.DisposeAsync().ConfigureAwait(false);
                        _buffer.Clear();
                    }

                    _disposed = true;
                }
            }

            private async IAsyncEnumerator<T> GetAsyncEnumerator_()
            {
                var i = 0;

                try
                {
                    while (true)
                    {
                        if (_disposed)
                            throw new ObjectDisposedException("");

                        var hasValue = default(bool);
                        var current = default(T)!;

                        using (await _gate.LockAsync().ConfigureAwait(false))
                        {
                            if (i >= _buffer.Count)
                            {
                                if (!_stopped)
                                {
                                    try
                                    {
                                        hasValue = await _source.MoveNextAsync().ConfigureAwait(false);
                                        if (hasValue)
                                            current = _source.Current;
                                    }
                                    catch (Exception ex)
                                    {
                                        _stopped = true;
                                        _error = ex;

                                        await _source.DisposeAsync().ConfigureAwait(false);
                                    }
                                }

                                if (_stopped)
                                {
                                    if (_error != null)
                                        throw _error;
                                    else
                                        break;
                                }

                                if (hasValue)
                                {
                                    _buffer.Add(current);
                                }
                            }
                            else
                            {
                                hasValue = true;
                            }
                        }

                        if (hasValue)
                            yield return _buffer[i];
                        else
                            break;

                        i++;
                    }
                }
                finally
                {
                    if (_buffer != null)
                        _buffer.Done(i + 1);
                }
            }
        }
    }

    internal interface IRefCountList<T>
    {
        void Clear();

        int Count { get; }

        T this[int i] { get; }

        void Add(T item);

        void Done(int index);
    }

    internal sealed class RefCountList<T> : IRefCountList<T>
    {
        private readonly IDictionary<int, RefCount> _list;

        public RefCountList(int readerCount)
        {
            ReaderCount = readerCount;
            _list = new Dictionary<int, RefCount>();
        }

        public int ReaderCount { get; set; }

        public void Clear() => _list.Clear();

        public int Count { get; private set; }

        public T this[int i]
        {
            get
            {
                if (!_list.TryGetValue(i, out var res))
                    throw new InvalidOperationException("Element no longer available in the buffer.");

                var val = res.Value;

                if (--res.Count == 0)
                {
                    _list.Remove(i);
                }

                return val;
            }
        }

        public void Add(T item)
        {
            _list[Count] = new RefCount(item, ReaderCount);

            Count++;
        }

        public void Done(int index)
        {
            for (var i = index; i < Count; i++)
            {
                _ = this[i];
            }

            ReaderCount--;
        }

        private sealed class RefCount
        {
            public RefCount(T value, int count)
            {
                Value = value;
                Count = count;
            }

            public int Count { get; set; }
            public T Value { get; }
        }
    }

    internal sealed class MaxRefCountList<T> : IRefCountList<T>
    {
        private readonly IList<T> _list = new List<T>();

        public void Clear() => _list.Clear();

        public int Count => _list.Count;

        public T this[int i] => _list[i];

        public void Add(T item) => _list.Add(item);

        public void Done(int index) { }
    }

    /// <summary>
    /// Represents a buffer exposing a shared view over an underlying enumerable sequence.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IBuffer<out T> : IAsyncEnumerable<T>, IAsyncDisposable
    {
    }

    public sealed class AsyncLock
    {
        private readonly object _gate = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly AsyncLocal<int> _recursionCount = new AsyncLocal<int>();

        public ValueTask<Releaser> LockAsync()
        {
            var shouldAcquire = false;

            lock (_gate)
            {
                if (_recursionCount.Value == 0)
                {
                    shouldAcquire = true;
                    _recursionCount.Value = 1;
                }
                else
                {
                    _recursionCount.Value++;
                }
            }

            if (shouldAcquire)
            {
                return new ValueTask<Releaser>(_semaphore.WaitAsync().ContinueWith(_ => new Releaser(this)));
            }

            return new ValueTask<Releaser>(new Releaser(this));
        }

        private void Release()
        {
            lock (_gate)
            {
                if (--_recursionCount.Value == 0)
                {
                    _semaphore.Release();
                }
            }
        }

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _parent;

            public Releaser(AsyncLock parent) => _parent = parent;

            public void Dispose() => _parent.Release();
        }
    }
}