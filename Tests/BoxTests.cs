using Tests.TestHelpers;

namespace Tests
{
 
    using NUnit.Framework;
    using ThreadZipper;
    using System.IO;
    using System;

    [TestFixture]
    class BoxTests
    {
        private string _srcFilepath;
        private string _zippedFilePath;
        private string _unzippedFilePath;

        [SetUp]
        public void Setup()
        {
            
            _zippedFilePath = Directory.GetCurrentDirectory() + @"\testVeeamZipped";
            
            _unzippedFilePath = Directory.GetCurrentDirectory() + @"\testVeeamUnzipped";
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_srcFilepath))
            {
                File.Delete(_srcFilepath);
            }
            if (File.Exists(_zippedFilePath))
            {
                File.Delete(_zippedFilePath);
            }
            if (File.Exists(_unzippedFilePath))
            {
                File.Delete(_unzippedFilePath);
            }
        }

        [TestCase(100)]
        [TestCase(10_000)]
        [TestCase(1_000_000)]
       // [TestCase(12_345_6791_234)]
        public void RandomFileTest(int srcFileLength)
        {
            _srcFilepath = RandomFileHelper.CreateTempFile(srcFileLength);
            _zippedFilePath = RandomFileHelper.CreateTempFile(1);
            GC.Collect();
            ProgramManager _manager = new ProgramManager(_srcFilepath, _zippedFilePath, "Compress");
            _manager.Compress();

            _manager = new ProgramManager(_zippedFilePath, _unzippedFilePath, "Decompress");
            _manager.Compress();

            FileInfo srcFileInfo = new FileInfo(_srcFilepath);
            FileInfo resultFileInfo = new FileInfo(_unzippedFilePath);

            Assert.AreEqual(srcFileInfo.Length, resultFileInfo.Length);
        }
    }
}
