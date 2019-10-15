namespace Tests.ConsoleZipper
{
    using Common;
    using global::ConsoleZipper.Workers;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    [TestFixture]
    public class ReaderTests
    {       

        [SetUp]
        public void SetUp()
        {
            
        }

        private FileInfo CreateTempFile(long count)
        {
            var path = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(path);

            var r = new Random();
            var testData = new byte[count];
            r.NextBytes(testData);

            using (var wr = fileInfo.OpenWrite())
            {
                wr.Write(testData, 0, testData.Length);
            }

            return fileInfo;
        }

        [TearDown]
        public void TearDown()
        {
            
        }


        [TestCase(10)]
        [TestCase(1000)]
        [TestCase(1000000)]
        [TestCase(10000000)]
        public void Test(long count)
        {
            FileInfo fileInfo = CreateTempFile(count);

            Reader reader = new Reader(fileInfo);
            Queue<Datablock> queue = new Queue<Datablock>();
            reader.DataReceived += (obj, args) => 
            {
                queue.Enqueue(args.Datablock);
            };            
            reader.Work();

            using (var wr = fileInfo.OpenRead())
            {
                while (queue.TryDequeue(out var block))
                {
                    byte[] awaitData = new byte[block.Count];
                    wr.Read(awaitData, 0, block.Count);

                    byte[] actualData = new byte[block.Count];
                    Array.Copy(block.Data, actualData, block.Count);
                    CollectionAssert.AreEqual(awaitData, actualData);
                }
                Assert.AreEqual(fileInfo.Length, wr.Position);
            }

            File.Delete(fileInfo.FullName);
        }

    }
}
