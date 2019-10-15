using Common;
using ConsoleZipper.Workers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConsoleZipper
{
    class Program
    {
        static string SRC_FILEPATH = @"E:\Veeam\test.iso";
        static string ZIP_FILEPATH = @"E:\Veeam\ziped.iso";
        static string UNZIP_FILEPATH = @"E:\Veeam\unziped.iso";

        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            FileInfo srcInfo = new FileInfo(SRC_FILEPATH);
            File.Create(@"E:\Veeam\testOut.iso");
            File.Create(@"E:\Veeam\testOut.iso");
            FileInfo trgInfo = new FileInfo(@"E:\Veeam\testZipped.iso");

            Reader reader = new Reader(srcInfo);
            Writter writter = new Writter(trgInfo);
            Zipper zipper = new Zipper();

            Queue<Datablock> readQueue = new Queue<Datablock>();
            Queue<Datablock> writeQueue = new Queue<Datablock>();

            Thread[] threads = new Thread[3];

            Thread writerThread = new Thread(() => { 

            });

            reader.Completed += (obj, arg) => 
            {
                
            };
            reader.DataReceived += (obj, arg) => 
            {
               // new Thread(() => writeQueue.Enqueue(arg.Datablock) { }).Start();
            };

            reader.Work();




            Console.WriteLine("Ended");
        }

    }
}
