﻿using Common.ComandManager;

namespace ThreadZipper
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            // args = new[] {"d:\\srcFile.iso", "d:\\zippedFile.iso", "Compress"};
            // args = new[] {"d:\\zippedFile.iso", "d:\\unzipped.iso", "Decompress"};
            if (CommandManager.Verify(args, out var errMessage))
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