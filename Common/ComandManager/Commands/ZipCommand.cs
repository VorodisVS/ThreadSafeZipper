using System;
using System.IO;
using System.Threading;
using Common.Workers;

namespace Common.ComandManager.Commands
{
    public class CompressCommand : ZipCommandBase
    {
        public CompressCommand(params string[] args)
        {
            SrcFilepath = args[1];
            TrgFilepath = args[2];
        }

        public override bool IsCompress => true;
    }

    public class DecompressCommand : ZipCommandBase
    {
        public DecompressCommand(params string[] args)
        {
            SrcFilepath = args[1];
            TrgFilepath = args[2];
        }

        public override bool IsCompress => false;
    }

    public abstract class ZipCommandBase : ICommand
    {
        public string SrcFilepath { get; protected set; }
        public string TrgFilepath { get; protected set; }
        public virtual bool IsCompress { get; protected set; }
        public event EventHandler<ExceptionEventArgs> ErrorOccured;

        public void Execute()
        {
            using (var srcStream = File.OpenRead(SrcFilepath))
            {
                using (var zipStream = File.OpenWrite(TrgFilepath))
                {
                    Zip(srcStream, zipStream);
                }
            }
        }

        private void Zip(Stream readStream, Stream writeStream)
        {
            var threadCount = Environment.ProcessorCount > 3 ? Environment.ProcessorCount : 3;
            var threads = new Thread[threadCount];

            IDataCollection readCollection = new SafeDataCollection();
            IDataCollection writeCollection = new SafeDataCollection();

            // Create workers
            var reader = new Reader(readCollection);
            var writer = new Writer(writeCollection);
            var zippers = new Zipper[threads.Length - 2];
            for (var i = 0; i < threads.Length - 2; i++)
                zippers[i] = new Zipper(readCollection, writeCollection, IsCompress);

            // Start workers in different threads
            threads[0] = new Thread(() => { DoAction(() => reader.Start(readStream, !IsCompress)); });

            threads[1] = new Thread(() => { DoAction(() => writer.Start(writeStream)); });

            for (var i = 2; i < threads.Length; i++)
            {
                var curNum = i - 2;
                threads[i] = new Thread(() => { DoAction(() => zippers[curNum].Start()); });
            }

            foreach (var thread in threads)
                thread.Start();

            // Stop workers
            threads[0].Join();
            foreach (var zipper in zippers)
                zipper.Stop();

            readCollection.ReleaseAllWaiters(threads.Length);

            for (var i = 2; i < threads.Length; i++) threads[i].Join();

            writer.Stop();
            writeCollection.ReleaseAllWaiters(threads.Length);
            threads[1].Join();
        }

        private void DoAction(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }
    }
}