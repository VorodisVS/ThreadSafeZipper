namespace MultiThreadZip.Workers
{
    using Common;
    using MultiThreadZip.SyncObjects;

    public class ZipWorker :IWorker
    {
        private IBlockProvider _blockProvider;

        #region Constructors

        public ZipWorker(IBlockProvider blockProvider)
        {
            _blockProvider = blockProvider;
        }

        #endregion

        #region Methods

        public void Work(int blockNumber)
        {
            BlockArchiver ar = new BlockArchiver();
          
        }

        #endregion
    }
}
