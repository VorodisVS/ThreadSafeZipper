namespace ConsoleZipper.Workers
{
    using Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class Writter
    {
        BlockWriter _blockWriter;
        FileInfo _fileInfo;
        long _currentPosition;
        object _locker;
        private int _lastWritedIndex;

        SortedList<int, Datablock> _blockQueue;
        

        public Writter(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            _currentPosition = 0;
            _lastWritedIndex = 0;
            _blockWriter = new BlockWriter(fileInfo.FullName);
        }

        public void Write(Datablock block)
        {
            if (_lastWritedIndex == block.Number && Monitor.TryEnter(_locker))
            {
                _blockWriter.Write(block, _currentPosition);
                _currentPosition += block.Count;
                _lastWritedIndex = block.Number;

            }
            else
            {
                _blockQueue.Add(block.Number, block);
            }

        }
    }
}
