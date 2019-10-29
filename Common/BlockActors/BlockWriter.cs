namespace Common.BlockActors
{
    using System.IO;

    public class BlockWriter
    {
        #region Fields

        private readonly string _filepath;

        #endregion

        #region Constructors

        public BlockWriter(string filepath)
        {
            _filepath = filepath;
        }

        #endregion

        #region Methods

        public void Write(Datablock block, long startPosition)
        {
            using (var fstream = File.OpenWrite(_filepath))
            {
                using (var wr = new BinaryWriter(fstream))
                {
                    wr.BaseStream.Position = startPosition;
                    wr.Write(block.Data, 0, block.Count);
                }
            }
        }

        public static void Write(Stream stream, Datablock block, long startPosition)
        {
            using (var wr = new BinaryWriter(stream))
            {
                wr.BaseStream.Position = startPosition;
                wr.Write(block.Data, 0, block.Count);
            }
        }

        #endregion
    }
}