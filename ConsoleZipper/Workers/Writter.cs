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

        SortedDictionary<int, Datablock> nonWriteDictionary = new SortedDictionary<int, Datablock>();

        public Writter(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            _currentPosition = 0;
            _lastWritedIndex = 0;
            _blockWriter = new BlockWriter(fileInfo.FullName);
        }

        public void Write2(Datablock block)
        {
            lock (nonWriteDictionary)
            {
                nonWriteDictionary.Add(block.Number, block);
            }
            if (Monitor.TryEnter(_locker))
            {
                try
                {
                    WriteInternal();
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
            }
        }

        private void WriteInternal()
        {
            List<int> writedIndexes = new List<int>();

            // TODO Fix unsafe reading from dictionary
            foreach (var keyValue in nonWriteDictionary)
            {
                if (_lastWritedIndex == keyValue.Key)
                {
                    _blockWriter.Write(keyValue.Value, _currentPosition);
                    _currentPosition += keyValue.Value.Count;
                    _lastWritedIndex = keyValue.Value.Number;
                    writedIndexes.Add(keyValue.Key);
                }
                else break;
            }

            lock (nonWriteDictionary)
            {
                foreach (var writedIndex in writedIndexes)
                {
                    nonWriteDictionary.Remove(writedIndex);
                }
            }
        }

        public void Write(Datablock block)
        {
            if (Monitor.TryEnter(_locker))
            {
                if (_lastWritedIndex == block.Number)
                {
                    _blockWriter.Write(block, _currentPosition);
                    _currentPosition += block.Count;
                    _lastWritedIndex = block.Number;
                }
                else
                {
                    _blockQueue.Add(block.Number, block);
                }
                Monitor.Exit(_locker);
            }
            else
            {
                _blockQueue.Add(block.Number, block);
            }
        }
    }
}
