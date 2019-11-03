using System;

namespace Common.ComandManager
{
    public interface ICommand
    {
        event EventHandler<ExceptionEventArgs> ErrorOccured;
        void Execute();
    }

    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; }
    }
}