using System;
using System.Collections.Generic;
using System.Threading;
using Common;

namespace ThreadZipper.Workers
{
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

        private readonly int _curId;
        private readonly AutoResetEvent _resetEvent;


        public SafeDataCollection(int number)
        {
            _locker = new object();
            _queue = new Queue<Datablock>();
            _curId = number;
            _resetEvent = new AutoResetEvent(false);
        }

        public void Enqueue(Datablock block)
        {
            lock (_locker)
            {
              //  Console.WriteLine($"{_curId} - Enqueued {block.Number} ");
                _queue.Enqueue(block);
                _resetEvent.Set();
            }
        }

        public Datablock Dequeue()
        {
            if (_queue.Count == 0)
                _resetEvent.Reset();
            CheckLock();

            lock (_locker)
            {
                if (!_queue.TryDequeue(out var block)) return null;

                if (_queue.Count != 0)
                    _resetEvent.Set();
              //  Console.WriteLine($"{_curId} - Dequeued {block.Number}");
                return block;
            }
        }

        public void ReleaseAllWaiters(int maxWaiters)
        {
            while (maxWaiters-- != 0)
                _resetEvent.Set();
        }

        public int Count => _queue.Count;


        private void CheckLock()
        {
            _resetEvent.WaitOne();
        }
    }
}