namespace MultiThreadZip.Workers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWorker
    {         
        void Work(int blockNumber);
    }
}
