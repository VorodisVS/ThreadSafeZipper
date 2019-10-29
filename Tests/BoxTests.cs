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
            
            _zippedFilePath = Path.GetTempPath() + @"testVeeamZipped";            
            _unzippedFilePath = Path.GetTempPath() + @"testVeeamUnzipped";
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
        [TestCase(12_345_6791_234)]
        public void RandomFileTest(long srcFileLength)
        {
            _srcFilepath = RandomFileHelper.CreateTempFile(srcFileLength);
            ProgramManager _manager = new ProgramManager(_srcFilepath, _zippedFilePath, "Compress");

            using (var srcStream = File.OpenRead(_srcFilepath))
            {
                using (var zipStream = File.OpenWrite(_zippedFilePath))
                {
                    _manager.Compress(srcStream, zipStream);
                }
            }

            

            _manager = new ProgramManager(_zippedFilePath, _unzippedFilePath, "Decompress");


            using (var zipStream = File.OpenRead(_zippedFilePath))
            {
                using (var unzipStream = File.OpenWrite(_unzippedFilePath))
                {
                    _manager.Compress(zipStream, unzipStream);
                }
            }           

            FileInfo srcFileInfo = new FileInfo(_srcFilepath);
            FileInfo resultFileInfo = new FileInfo(_unzippedFilePath);

            Assert.AreEqual(srcFileInfo.Length, resultFileInfo.Length);
        }
    }
}
