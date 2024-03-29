﻿using System.IO;
using System.IO.Compression;

namespace Common.BlockActors
{
    public class BlockArchiver : IBlockZipper
    {
        #region Methods

        public void Zip(Datablock src, Datablock trg)
        {
            using (var targetStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress, true))
                {
                    compressionStream.Write(src.Data, 0, src.Count);
                }

                ArchiverHelper.GetHeaderWithLength(targetStream, trg);
            }
        }

        #endregion
    }

    public class BlockDearchiver : IBlockZipper
    {
        #region Methods

        public void Zip(Datablock src, Datablock trg)
        {
            var oLength = 0;
            for (var i = 1; i <= 4; i++)
                oLength = (oLength << 8) | src.Data[src.Count - i];
            trg.Count = oLength;

            using (var targetStream = new MemoryStream(src.Data))
            {
                using (var compressionStream = new GZipStream(targetStream, CompressionMode.Decompress))
                {
                    compressionStream.Read(trg.Data, 0, trg.Count);
                }
            }
        }

        #endregion
    }

    public interface IBlockZipper
    {
        void Zip(Datablock src, Datablock trg);
    }
}