using Common;

namespace MultiThreadZip.SyncObjects
{   

    public interface IBlockProvider
    {
        void Release(Datablock item);
        Datablock Take();
    }
}
