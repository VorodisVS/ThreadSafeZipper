using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace ThreadZipper.Workers
{
    public class Writer
    {
        private readonly BlockWriter _blockWriter;
        private int _curBlockNumber;

        private long _curByteIndex;

        private readonly SortedList<int, Datablock> _delayedBlocks;
        private readonly FileInfo _fileInfo;
        private readonly IDataCollection _internalCollection;

        private bool _stopDetected;


        public Writer(IDataCollection collection, FileInfo info)
        {
            _fileInfo = info;
            _internalCollection = collection;
            _delayedBlocks = new SortedList<int, Datablock>();
            _blockWriter = new BlockWriter(_fileInfo.FullName);
        }

        public void Start()
        {
           // Console.WriteLine("Writter started");
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
                    //Console.WriteLine($"Writted {block.Number}");
                }

                WriteDelayed();
            }

           // Console.WriteLine("Writter stoped");
        }

        private void WriteDelayed()
        {
            if (_delayedBlocks.Count == 0)
                return;
            var firstBlock = _delayedBlocks.ElementAt(0).Value;
            if (_curBlockNumber != firstBlock.Number)
            {
            }
            else
            {
                _blockWriter.Write(firstBlock, _curByteIndex);
                _curBlockNumber++;
            //    Console.WriteLine($"Writted delayed {firstBlock.Number}");
                _curByteIndex += firstBlock.Count;
                _delayedBlocks.RemoveAt(0);
                WriteDelayed();
            }
        }

        public void Stop()
        {
            _stopDetected = true;
        }
    }
}