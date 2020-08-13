using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stream = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n","o","p" };
            System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();
            _watch.Start();
            //Partition + Schedule + Collate
            var encryptedList = stream.AsParallel().Select(item => Encrypt(item)).ToList();
            _watch.Stop();
            Console.WriteLine($"Elapsed time {_watch.ElapsedMilliseconds}");
            foreach(var item in encryptedList)
            {
                Console.WriteLine(item);
            }

            //Range Partition
            stream.AsParallel().WithDegreeOfParallelism(2).Select(item => Encrypt(item)).ForAll((item) => {

            Console.WriteLine($"Item {item} processed by {System.Threading.Thread.CurrentThread.ManagedThreadId}");
              });



            //Chunk Partition / Dynamic Parttion

            Partitioner.Create(stream,true).AsParallel().WithDegreeOfParallelism(2).Select(item => Encrypt(item)).ForAll((item) => {

            Console.WriteLine($"Item {item} processed by {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                  });
        }
        static string Encrypt(string item)
        {
            if (item == "c")
            {
                System.Threading.Thread.Sleep(15000);
            }
            System.Threading.Thread.Sleep(2000);
            return item.ToUpper();
        }
    }
}
