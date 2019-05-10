﻿namespace Tests
{
    using System;
    using System.IO;

    using MultiThreadZip;

    using NUnit.Framework;

    [TestFixture]
    public class ArchiverHelperTests
    {
        #region Methods

        [TestCase(10)]
        [TestCase(0xFF)]
        [TestCase(0xFFFF)]
        [TestCase(0xFFFFF6)]
        public void TestHeaderWithLegthAndLenthTest(int length)
        {
            var r = new Random();

            var srcArray = new byte[length];
            srcArray[0] = 31;
            srcArray[1] = 139;
            srcArray[2] = 8;
            srcArray[3] = 0;
            srcArray[4] = 0;
            srcArray[5] = 0;
            srcArray[6] = 0;
            srcArray[7] = 0;
            srcArray[8] = 4;
            srcArray[9] = 0;
            for (var i = 10; i < length; i++)
                srcArray[i] = (byte)r.Next();

            byte[] arrayWithHeader;
            using (var ms = new MemoryStream(srcArray))
            {
                arrayWithHeader = ArchiverHelper.GetHeaderWithLength(ms);
            }

            Assert.AreEqual(srcArray.Length + ArchiverHelper.EXTRA_FIELD_LENGTH, arrayWithHeader.Length);
            Assert.AreNotEqual(srcArray[3], arrayWithHeader[3]);

            var resultCount = 0;
            using (var ms = new MemoryStream(arrayWithHeader))
            {
                resultCount = ArchiverHelper.GetBlockLegth(ms);
            }

            Assert.AreEqual(arrayWithHeader.Length, resultCount);
        }

        #endregion
    }
}
