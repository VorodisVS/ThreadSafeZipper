namespace Common
{
    using System.IO;

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

        public bool Read(Datablock block, long startPosition, int count, bool forUnzip)
        {            
            using (FileStream fstream = File.OpenRead(_filepath))
            {
                using (var wr = new BinaryReader(fstream))
                {
                    if (forUnzip)
                    {
                        count = ArchiverHelper.GetBlockLegth(fstream, startPosition);
                    }
                    wr.BaseStream.Position = startPosition;
                    int realCount = wr.Read(block.Data, 0, count);
                    block.Count = realCount;
                }
            }            
            return true;
        }

        #endregion
    }
}
