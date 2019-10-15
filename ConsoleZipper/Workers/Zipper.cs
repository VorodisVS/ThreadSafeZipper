namespace ConsoleZipper.Workers
{
    using Common;

    public class Zipper
    {
        BlockArchiver _archiver;        

        public Zipper()
        {
            _archiver = new BlockArchiver();
        }

        public Datablock Zip(Datablock block)
        {
            Datablock datablock = new Datablock(block.Number)
            {
                Data = new byte[block.Count * 2],
            };
            _archiver.Compress(block, datablock);
            return datablock;
        }
    }

    public class UnZipper
    {
        BlockDearchiver _archiver;

        public UnZipper()
        {
            _archiver = new BlockDearchiver();
        }

        public Datablock Zip(Datablock block)
        {
            Datablock datablock = new Datablock(block.Number)
            {
                Data = new byte[block.Count * 2],
            };
            _archiver.Decompress(block, datablock);
            return datablock;
        }
    }
}
