namespace Common.BlockActors
{
    public class Datablock
    {
        #region Constants

        public const int MAX_BLOCK_SIZE = 1_000_000;

        #endregion

        #region Properties

        public int Count { get; set; }
        public int Number { get; set; }
        public byte[] Data { get; set; }

        public Datablock(int number)
        {
            Data = new byte [MAX_BLOCK_SIZE * 2];
            Number = number;
        }

        #endregion Properties
    }
}