using System;
using Common;

namespace ThreadZipper.Workers
{
    public class Zipper
    {
        private readonly IDataCollection _readCollection;
        private readonly IDataCollection _writeCollection;

        private readonly IBlockZipper _zipper;

        private readonly int _id;

        private bool _stopDetected;

        public Zipper(IDataCollection readCollection, IDataCollection writeCollection, bool isZip, int id)
        {
            _readCollection = readCollection;
            _writeCollection = writeCollection;
            if (isZip)
                _zipper = new BlockArchiver();
            else
                _zipper = new BlockDearchiver();

            _id = id;
        }

        public void Start()
        {
         //   Console.WriteLine($"Zipper {_id} started");
            while (!_stopDetected || _readCollection.Count > 0)
            {
                var srcBlock = _readCollection.Dequeue();
                if (srcBlock == null)
                    continue;

             //   Console.WriteLine($"Zipper {_id} received {srcBlock.Number}");
                var trgBlock = new Datablock(srcBlock.Number);
                _zipper.Zip(srcBlock, trgBlock);
            //    Console.WriteLine($"Zipper {_id} zipped {srcBlock.Number}");
                _writeCollection.Enqueue(trgBlock);
             //   Console.WriteLine($"Zipper {_id} transmitted {srcBlock.Number}");
            }

          //  Console.WriteLine($"Zipper {_id} stopped");
        }

        public void Stop()
        {
            _stopDetected = true;
        }
    }
}