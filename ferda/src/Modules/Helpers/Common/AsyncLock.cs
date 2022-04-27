using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferda.Modules.Helpers.Common
{
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
