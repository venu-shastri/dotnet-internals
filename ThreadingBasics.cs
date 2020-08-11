using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadingDemo
{
    class Program
    {
        static void Main1(string[] args)
        {

            //Explicit Threads - Foreground Mode
            Thread _t1 = new Thread(new ThreadStart(Program.ForegroundTask));
            Thread _t2 = new Thread(new ThreadStart(Program.BackgroundTask)) { IsBackground = true };
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) => { Program.NewBackgroundTask(); }));
           
            _t1.Start();
            _t2.Start();


        }

        static void  ForegroundTask()
        {
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine($"forground task {i} running, Mode Of Execution {Thread.CurrentThread.IsBackground}");
                Thread.Sleep(1000);
            }
            Console.WriteLine("Foreground Task Completed");
        }

        static void BackgroundTask()
        {
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine($"BackGround task {i} running , Mode Of Execution {Thread.CurrentThread.IsBackground}");
                Thread.Sleep(2000);
                if (i == 4)
                {
                    Console.ReadKey();
                }
            }
            Console.WriteLine("Background Task Completed");
        }
        static void NewBackgroundTask()
        {
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine($"New BackGround task {i} running , Are You From ThreadPool {Thread.CurrentThread.IsThreadPoolThread}");
                Thread.Sleep(2000);
                
            }
            Console.WriteLine("Background Task Completed");
        }

        static void Main()
        {
            new Thread(Singleton.Instance.Increment) { Name = "T1" }.Start();
            new Thread(Singleton.Instance.Decrement) { Name = "T2" }.Start();
        }
    }

    public class Singleton
    {

        int state;

        public static readonly Singleton Instance = null;

        static Singleton()
        {
            Instance = new Singleton();
        }
        private Singleton()
        {

        }

        public void Increment() { 
        
                    for(int i = 0; i < 15; i++) {
                            state++;
                Console.WriteLine($"State incremented By {Thread.CurrentThread.Name} and state Value {state}");
                Thread.Sleep(2000);
                    }
            }
        public void Decrement() {
            for(int i = 0; i < 15; i++)
            {
                state--;
                Console.WriteLine($"State Decremented By {Thread.CurrentThread.Name} and state Value {state}");
                Thread.Sleep(5000);
            }
        }
    }
}
