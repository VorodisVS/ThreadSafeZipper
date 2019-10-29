namespace Tests.TestHelpers
{
    using System;
    using System.IO;

    public class RandomFileHelper
    {
        private const long MAX_RANDOM_DATA_SIZE = 5_000_000;

        public static string CreateTempFile(long size)
        {
            var path = Path.GetTempFileName();

            var chunkCount = size / MAX_RANDOM_DATA_SIZE;

            byte[] data = new byte[size];
            Random r = new Random();
            r.NextBytes(data);
            WriteToFile(path, data);

            return path;
        }

        private static void WriteToFile(string filePath, byte[] data)
        {
            using (FileStream fstream = File.OpenWrite(filePath))
            {
                using (var wr = new BinaryWriter(fstream))
                {
                    wr.BaseStream.Position = 0;
                    wr.Write(data, 0, data.Length);
                }
            }
        }
    }

}
