namespace ConsoleZipper.Workers
{
    using Common;
    using System;
    using System.IO;
    using System.Threading;

    public class Reader
    {
        private readonly BlockReader _reader;
        private int COUNT = 1_000_000;
        private long _curByteIndex;
        private int _curBlockIndex;
        private FileInfo _info;

        private ManualResetEvent _resetEvent;

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<EventArgs> Completed;

        public Reader(FileInfo info)
        {
            _info = info;
            _curByteIndex = 0;
            _curBlockIndex = 0;
            _reader = new BlockReader(info.FullName);
            _resetEvent = new ManualResetEvent(true);
        }

        public void Work()
        {
            while (_info.Length > _curByteIndex)
            {
                _resetEvent.WaitOne();
                Datablock block = new Datablock(_curBlockIndex)
                {
                    Data = new byte[COUNT],
                };
                _reader.Read(block, _curByteIndex, COUNT, true);
                _curByteIndex += block.Count;
                DataReceived?.Invoke(this, new DataReceivedEventArgs(block));
            }
            Completed?.Invoke(this, new EventArgs());
        }

        public void SetLockState(bool state)
        {
            if (state)
                _resetEvent.Set();
            else
                _resetEvent.Reset();
        }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public Datablock Datablock { get; }

        public DataReceivedEventArgs(Datablock block)
        {
            Datablock = block;
        }
    }
}
