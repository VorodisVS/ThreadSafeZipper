using System;
using System.IO;
using System.Threading;
using Common;

namespace ThreadZipper.Workers
{
    public class Reader
    {
        private BlockReader _blockReader;
        private int _curBlockNumber;

        private long _curByteIndex;
        private readonly FileInfo _fileInfo;
        private readonly IDataCollection _internalCollection;

        public Reader(IDataCollection collection, FileInfo info)
        {
            _fileInfo = info;
            _internalCollection = collection;
        }

        public void Start(bool forUnzip)
        {
           // Console.WriteLine("Reader Started");
            _blockReader = new BlockReader(_fileInfo.FullName);

            while (_fileInfo.Length > _curByteIndex)
            {
                if (_internalCollection.Count > 30)
                {
              //      Console.WriteLine("Reader Waiting");
                    Thread.Sleep(1000);
                    continue;
                }

                var block = new Datablock(_curBlockNumber);
                _blockReader.Read(block, _curByteIndex, 1000000, forUnzip);
                _curBlockNumber++;
                _curByteIndex += block.Count;
                _internalCollection.Enqueue(block);
            }

           // Console.WriteLine("Reader Stopped");
        }
    }
}