using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.BlockActors;

namespace Common.Workers
{
    public class Writer
    {
        private readonly SortedList<int, Datablock> _delayedBlocks;
        private readonly IDataCollection _internalCollection;
        private int _curBlockNumber;
        private long _curByteIndex;

        private bool _stopDetected;


        public Writer(IDataCollection collection)
        {
            _internalCollection = collection;
            _delayedBlocks = new SortedList<int, Datablock>();
        }

        public void Start(Stream stream)
        {
            while (!_stopDetected || _internalCollection.Count > 0)
            {
                var block = _internalCollection.Dequeue();

                if (block == null)
                    continue;

                if (_curBlockNumber != block.Number)
                {
                    _delayedBlocks.Add(block.Number, block);
                }
                else
                {
                    BlockWriter.Write(stream, block, _curByteIndex);
                    _curBlockNumber++;
                    _curByteIndex += block.Count;
                }

                WriteDelayed(stream);
            }
        }

        private void WriteDelayed(Stream stream)
        {
            if (_delayedBlocks.Count == 0)
                return;

            var firstBlock = _delayedBlocks.ElementAt(0).Value;
            if (_curBlockNumber != firstBlock.Number)
                return;

            BlockWriter.Write(stream, firstBlock, _curByteIndex);
            _curBlockNumber++;

            _curByteIndex += firstBlock.Count;
            _delayedBlocks.RemoveAt(0);
            WriteDelayed(stream);
        }

        public void Stop()
        {
            _stopDetected = true;
        }
    }
}