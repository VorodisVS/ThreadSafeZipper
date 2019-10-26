using Common.BlockActors;

namespace ThreadZipper.Workers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Common;

    public class Writer
    {
        private readonly BlockWriter _blockWriter;
        private int _curBlockNumber;
        private long _curByteIndex;

        private readonly SortedList<int, Datablock> _delayedBlocks;
        private readonly IDataCollection _internalCollection;

        private bool _stopDetected;


        public Writer(IDataCollection collection, FileInfo info)
        {
            _internalCollection = collection;
            _delayedBlocks = new SortedList<int, Datablock>();
            _blockWriter = new BlockWriter(info.FullName);
        }

        public void Start()
        {
            while (!_stopDetected || _internalCollection.Count > 0)
            {
                var block = _internalCollection.Dequeue();

                if (_curBlockNumber != block.Number)
                {
                    _delayedBlocks.Add(block.Number, block);
                }
                else
                {
                    _blockWriter.Write(block, _curByteIndex);
                    _curBlockNumber++;
                    _curByteIndex += block.Count;
                }

                WriteDelayed();
            }
        }

        private void WriteDelayed()
        {
            if (_delayedBlocks.Count == 0)
                return;

            var firstBlock = _delayedBlocks.ElementAt(0).Value;
            if (_curBlockNumber != firstBlock.Number)
                return;

            _blockWriter.Write(firstBlock, _curByteIndex);
            _curBlockNumber++;

            _curByteIndex += firstBlock.Count;
            _delayedBlocks.RemoveAt(0);
            WriteDelayed();
        }

        public void Stop()
        {
            _stopDetected = true;
        }
    }
}