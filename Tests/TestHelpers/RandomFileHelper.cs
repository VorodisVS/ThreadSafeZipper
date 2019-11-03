namespace Tests.TestHelpers
{
    using System;
    using System.IO;

    public class RandomFileHelper
    {
        private const long MAX_RANDOM_DATA_SIZE = 10_000_000;

        public static string CreateTempFile(long size)
        {
            var path = Path.GetTempFileName();
            byte[] data = new byte[MAX_RANDOM_DATA_SIZE];
            Random r = new Random();
            r.NextBytes(data);

            using (FileStream fstream = File.OpenWrite(path))
            {
                using (var wr = new BinaryWriter(fstream))
                {
                    while (size > 0)
                    {
                        long chunkCount = size > MAX_RANDOM_DATA_SIZE ? MAX_RANDOM_DATA_SIZE : size;
                        size -= MAX_RANDOM_DATA_SIZE;
                        wr.Write(data, 0, (int)chunkCount);
                    }
                }
            }

            return path;
        }

        private static void WriteToFile(string filePath, byte[] data, int count)
        {
            using (FileStream fstream = File.OpenWrite(filePath))
            {
                using (var wr = new BinaryWriter(fstream))
                {
                    wr.BaseStream.Position = 0;
                    wr.Write(data, 0, count);
                }
            }
        }
    }

}
