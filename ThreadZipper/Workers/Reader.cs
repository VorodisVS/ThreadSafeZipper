namespace ThreadZipper.Workers
{
    using System.IO;
    using System.Threading;
    using Common.BlockActors;


    public class Reader
    {      
        private int _curBlockNumber;
        private long _curByteIndex;
        private readonly IDataCollection _internalCollection;

        public Reader(IDataCollection collection)
        {            
            _internalCollection = collection;
        }

        public void Start(Stream stream, bool forUnzip)
        {            
            long fullLength = stream.Length;
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