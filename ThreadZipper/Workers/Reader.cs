using Common.BlockActors;

namespace ThreadZipper.Workers
{
    using System.IO;
    using System.Threading;
    using Common;

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
            _blockReader = new BlockReader(_fileInfo.FullName);

            while (_fileInfo.Length > _curByteIndex)
            {
                if (_internalCollection.Count > 30)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var block = new Datablock(_curBlockNumber);
                _blockReader.Read(block, _curByteIndex, Datablock.MAX_BLOCK_SIZE, forUnzip);
                _curBlockNumber++;
                _curByteIndex += block.Count;
                _internalCollection.Enqueue(block);
            }
        }
    }
}