namespace ThreadZipper
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
           // args = new[] {"d:\\srcFile.iso", "d:\\zippedFile.iso", "Compress"};
            args = new[] {"d:\\zippedFile.iso", "d:\\unzipped.iso", "Decompress"};
#endif
            Console.WriteLine($"Hello World! {DateTime.Now}");
            var manager = new ProgramManager(args[0], args[1], args[2]);
           // manager.Compress();
            Console.WriteLine($"Hello World! {DateTime.Now}");
            Console.ReadLine();
        }
    }
}