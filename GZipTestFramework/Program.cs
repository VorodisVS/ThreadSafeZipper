namespace GZipTestFramework
{
    using Common.ComandManager;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            //// args = new[] {"d:\\srcFile.iso", "d:\\zippedFile.iso", "Compress"};
            //// args = new[] {"Compress", "src.txt", "cmprst.txt"};

            // args = new[] {"Decompress", "cmprst.txt", "decmprst.txt"};

            if (!CommandManager.Verify(args, out var errMessage))
            {
                Console.WriteLine($"Error : {errMessage}");
                return;
            }

            var command = CommandManager.GetCommand(args);
            command.ErrorOccured += (sender, eventArgs) => { Console.WriteLine($"{sender} => {eventArgs.Exception}"); };
            command.Execute();

            Console.WriteLine("Program successfully finished");
            Console.ReadLine();
        }
    }
}
