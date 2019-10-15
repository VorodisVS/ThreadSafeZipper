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
            Number = number;
        }

        #endregion
    }
}
