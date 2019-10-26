using Common.BlockActors;

namespace ThreadZipper.Workers
{
    using System;
    using Common;

    public class Zipper
    {
        private readonly IDataCollection _readCollection;
        private readonly IDataCollection _writeCollection;

        private readonly IBlockZipper _zipper;

        private bool _stopDetected;

        public Zipper(IDataCollection readCollection, IDataCollection writeCollection, bool isZip)
        {
            _readCollection = readCollection;
            _writeCollection = writeCollection;
            if (isZip)
                _zipper = new BlockArchiver();
            else
                _zipper = new BlockDearchiver();
        }

        public void Start()
        {
            while (!_stopDetected || _readCollection.Count > 0)
            {
                var srcBlock = _readCollection.Dequeue();
                if (srcBlock == null)
                    continue;

                var trgBlock = new Datablock(srcBlock.Number);
                _zipper.Zip(srcBlock, trgBlock);
                _writeCollection.Enqueue(trgBlock);
            }
        }

        public void Stop()
        {
            _stopDetected = true;
        }
    }
}