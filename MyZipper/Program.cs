using Common;
using System;
using System.IO;
using System.Threading;

namespace MyZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class ZipManager
    {
        private Thread[] _threads;
        public event EventHandler Ended;
        private ISafeQueue _internalReadQueue;
        private ISafeQueue _internalWriteQueue;

        public ZipManager(ISafeQueue readQueue, ISafeQueue writeQueue)
        {
            _internalReadQueue = readQueue;
            _internalWriteQueue = writeQueue;
        }

        public void Start()
        {
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(() =>
                {
                    BlockArchiver zipper = new BlockArchiver();
                    var srcBlock = _internalReadQueue.GetNextBlock();
                    var trgBlock = new Datablock(srcBlock.Number);
                    zipper.Zip(srcBlock, trgBlock);
                });
            }

        }

        public void Stop()
        {
            var stopThread = new Thread(() =>
            {
                foreach (var thread in _threads)
                {
                    thread.Join();
                }
                Ended?.Invoke(this, EventArgs.Empty);
            });
        }

        private class TheadEvntArgs : EventArgs
        {
            public int ThreadId { get; }

            public TheadEvntArgs()
            {
                
            }
        }
    }




    public class ReadManager
    {
        private const int BYTE_COUNT = 100000;
        private Thread _thread;

        private FileInfo _fileInfo;
        private ISafeQueue _internalQueue;

        private int _curBlockNumber;
        private long _curFilePosition;

        public event EventHandler Ended;

        public ReadManager(int threadCount, FileInfo fileInfo, ISafeQueue queue)
        {
            _fileInfo = fileInfo;
        }

        public void Start()
        {
            BlockReader reader = new BlockReader(_fileInfo.FullName);
            _thread = new Thread(() =>
            {
                while (_fileInfo.Length > _curFilePosition)
                {
                    Datablock block = new Datablock(_curBlockNumber);
                    reader.Read(block, _curFilePosition, BYTE_COUNT);
                    _curFilePosition += block.Count;
                    _curBlockNumber++;
                    _internalQueue.Enqueue(block);
                }

                Ended?.Invoke(this, EventArgs.Empty);
            });
            _thread.Start();
        }
    }
}
