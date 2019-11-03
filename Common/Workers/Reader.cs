using System.IO;
using System.Threading;
using Common.BlockActors;

namespace Common.Workers
{
    public class Reader
    {
        private readonly IDataCollection _internalCollection;
        private int _curBlockNumber;
        private long _curByteIndex;

        public Reader(IDataCollection collection)
        {
            _internalCollection = collection;
        }

        public void Start(Stream stream, bool forUnzip)
        {
            var fullLength = stream.Length;
            while (fullLength > _curByteIndex)
            {
                if (_internalCollection.Count > 30)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var block = new Datablock(_curBlockNumber);
                BlockReader.Read(stream, block, _curByteIndex, Datablock.MAX_BLOCK_SIZE, forUnzip);
                _curBlockNumber++;
                _curByteIndex += block.Count;
                _internalCollection.Enqueue(block);
            }
        }
    }
}