using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace MyZipper
{
    public class ThreadSafeQueue : ISafeQueue
    {
        public event EventHandler NextBlockReady;
        public event EventHandler OverflowDetected;
        public event EventHandler OverflowReleased;

        private SortedDictionary<int, Datablock> _internalCollection;

        public ThreadSafeQueue()
        {
            _internalCollection = new SortedDictionary<int, Datablock>();
        }

        public Datablock GetNextBlock()
        {
            return  new Datablock(1);
        }

        public void Enqueue(Datablock block)
        {
            _internalCollection.Add(block.Number, block);
        }
    }

    public interface ISafeQueue
    {
        event EventHandler NextBlockReady;
        event EventHandler OverflowDetected;
        event EventHandler OverflowReleased;

        Datablock GetNextBlock();
        void Enqueue(Datablock block);
    }


}
