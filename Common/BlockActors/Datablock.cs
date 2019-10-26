namespace Common
{
    public class Datablock
    {
        #region Properties

        public int Count { get; set; }
        public int Number { get; set; }
        public byte[] Data { get; set; }

        public Datablock(int number)
        {
            Data = new byte [2_000_000];
            Number = number;
        }

        #endregion
    }
}
