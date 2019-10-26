using Common.BlockActors;

namespace ThreadZipper.Workers
{
    using System.Collections.Generic;
    using System.Threading;
    using Common;

    public interface IDataCollection
    {
        int Count { get; }
        void Enqueue(Datablock block);
        Datablock Dequeue();
        void ReleaseAllWaiters(int maxWaiters);
    }

    public class SafeDataCollection : IDataCollection
    {
        private readonly object _locker;
        private readonly Queue<Datablock> _queue;

        private readonly AutoResetEvent _resetEvent;

        public int Count => _queue.Count;

        public SafeDataCollection()
        {
            _locker = new object();
            _queue = new Queue<Datablock>();
            _resetEvent = new AutoResetEvent(false);
        }

        public void Enqueue(Datablock block)
        {
            lock (_locker)
            {
                _queue.Enqueue(block);
                _resetEvent.Set();
            }
        }

        public Datablock Dequeue()
        {
            if (_queue.Count == 0)
                _resetEvent.Reset();

            _resetEvent.WaitOne();

            lock (_locker)
            {
                if (!_queue.TryDequeue(out var block))
                    return null;

                if (_queue.Count != 0)
                    _resetEvent.Set();
                return block;
            }
        }

        public void ReleaseAllWaiters(int maxWaiters)
        {
            while (maxWaiters-- > 0)
                _resetEvent.Set();
        }
    }
}