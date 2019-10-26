namespace ThreadZipper
{
    using System;
    using System.IO;
    using System.Threading;
    using Workers;

    public class ProgramManager
    {
        private readonly FileInfo _srcFileInfo;
        private readonly FileInfo _trgFileInfo;
        private readonly string _cmd;

        public ProgramManager(string srcFilePath, string trgFilePath, string cmd)
        {
            _srcFileInfo = new FileInfo(srcFilePath);

            if (!_srcFileInfo.Exists)
            {
                Console.WriteLine("FileNotExist");
                Console.ReadLine();
                return;
            }

            _cmd = cmd;


            if (File.Exists(trgFilePath)) File.Delete(trgFilePath);
            File.Create(trgFilePath);
            _trgFileInfo = new FileInfo(trgFilePath);
        }

        public void Compress()
        {
            bool isCompress = _cmd.Equals("Compress");
            var threads = new Thread[Environment.ProcessorCount];

            IDataCollection readCollection = new SafeDataCollection();
            IDataCollection writeCollection = new SafeDataCollection();

            var reader = new Reader(readCollection, _srcFileInfo);
            var writer = new Writer(writeCollection, _trgFileInfo);
            var zippers = new Zipper[threads.Length - 2];
            for (var i = 0; i < threads.Length - 2; i++)
                zippers[i] = new Zipper(readCollection, writeCollection, isCompress);

            threads[0] = new Thread(() => { reader.Start(!isCompress); });
            threads[1] = new Thread(() => { writer.Start(); });

            for (var i = 2; i < threads.Length; i++)
            {
                var curNum = i - 2;
                threads[i] = new Thread(() => { zippers[curNum].Start(); });
            }

            foreach (var thread in threads) thread.Start();

            threads[0].Join();
            foreach (var zipper in zippers) zipper.Stop();
            readCollection.ReleaseAllWaiters(threads.Length);

            for (var i = 2; i < threads.Length; i++) threads[i].Join();

            writer.Stop();

            threads[1].Join();
        }
    }
}