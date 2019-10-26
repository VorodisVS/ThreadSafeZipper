using System.IO;

namespace Common.BlockActors
{
    public class BlockReader
    {
        #region Fields

        private readonly string _filepath;

        #endregion

        #region Constructors

        public BlockReader(string filepath)
        {
            _filepath = filepath;
        }

        #endregion

        #region Methods

        public bool Read(Datablock block, long startPosition, int preferredCount, bool forUnzip)
        {
            using (var fstream = File.OpenRead(_filepath))
            {
                using (var wr = new BinaryReader(fstream))
                {
                    if (forUnzip) preferredCount = ArchiverHelper.GetBlockLength(fstream, startPosition);
                    wr.BaseStream.Position = startPosition;
                    var realCount = wr.Read(block.Data, 0, preferredCount);
                    block.Count = realCount;
                }
            }

            return true;
        }

        #endregion
    }
}