using System;
using System.IO;
using System.Threading;
using ThreadZipper.Workers;

namespace ThreadZipper
{
    public class ProgramManager
    {
        private readonly FileInfo _srcFileInfo;
        private readonly FileInfo _trgFileInfo;

        public ProgramManager(string srcFilePath, string trgFilePath, string cmd)
        {
            _srcFileInfo = new FileInfo(srcFilePath);

            if (!_srcFileInfo.Exists)
            {
                Console.WriteLine("FileNotExist");
                Console.ReadLine();
                return;
            }


            if (File.Exists(trgFilePath)) File.Delete(trgFilePath);
            File.Create(trgFilePath);
            _trgFileInfo = new FileInfo(trgFilePath);
        }

        public void Compress(bool isCompress)
        {
            var threads = new Thread[Environment.ProcessorCount * 3];

            IDataCollection readCollection = new SafeDataCollection(1);
            IDataCollection writeCollection = new SafeDataCollection(2);

            var reader = new Reader(readCollection, _srcFileInfo);
            var writer = new Writer(writeCollection, _trgFileInfo);
            var zippers = new Zipper[threads.Length - 2];
            for (var i = 0; i < threads.Length - 2; i++)
                zippers[i] = new Zipper(readCollection, writeCollection, isCompress, i);

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