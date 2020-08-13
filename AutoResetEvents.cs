using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadingDemo
{
    static class Events
    {
        public static ManualResetEvent MRE = new ManualResetEvent(false);
        public static AutoResetEvent ARE1 = new AutoResetEvent(false);
        public static AutoResetEvent ARE2 = new AutoResetEvent(false);

    }
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
            //new Thread(Singleton.Instance.Increment) { Name = "T1" }.Start();
            //new Thread(Singleton.Instance.Decrement) { Name = "T2" }.Start();
            //new Thread(Singleton.Instance.PrintState) { Name = "T3" }.Start();
            //new Thread(Singleton.Instance.PrintStateWithFormat) { Name = "T4" }.Start();

            Thread t1 = new Thread(Singleton_Mutex.Instance.Increment) { Name = "T1", IsBackground = true };
            Thread t2=new Thread(Singleton_Mutex.Instance.Decrement) { Name = "T2", IsBackground = true };
            Thread t3=new Thread(Singleton_Mutex.Instance.PrintState) { Name = "T3", IsBackground = true };
            Thread t4=new Thread(Singleton_Mutex.Instance.PrintStateWithFormat) { Name = "T4", IsBackground = true };

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            Console.WriteLine("Press Any Key To Resume  Threads");
            Console.ReadKey();

            Events.MRE.Set();

            WaitHandle.WaitAll(new WaitHandle[] { Events.ARE1,Events.ARE2 });
            Console.WriteLine("End Of Main");

        }
    }

    //[System.Runtime.Remoting.Contexts.Synchronization]
    public class Singleton //:ContextBoundObject // enable context and proxy 
    {
        object _syncObjForUpdate = new object();
        object _syncObjForPrint = new object();
        int state;

        public static readonly Singleton Instance = null;

        static Singleton()
        {
            Instance = new Singleton();
        }
        private Singleton()
        {

        }

        //[System.Runtime.CompilerServices.MethodImpl(
          //  System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Increment() {
            Monitor.Enter(_syncObjForUpdate);
            try
            {

                for (int i = 0; i < 5; i++)
                {
                    state++;
                    Console.WriteLine($"State incremented By {Thread.CurrentThread.Name}");
                    Thread.Sleep(2000);
                    if (i == 3)
                    {
                        return;
                    }
                }
            }
            finally
            {
                Monitor.Exit(_syncObjForUpdate);
                Events.ARE1.Set();
            }
           
            }
     //  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Decrement() {
            lock (_syncObjForUpdate)
            {
                for (int i = 0; i < 5; i++)
                {
                    state--;
                    Console.WriteLine($"State Decremented By {Thread.CurrentThread.Name}");
                    Thread.Sleep(5000);
                }
                Events.ARE2.Set();
            }
        }

      //  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void PrintState()
        {
            
            while (true)
            {
                Monitor.Enter(_syncObjForPrint);
                Console.WriteLine($"Current State {this.state} from Thread  {Thread.CurrentThread.Name}");
                Thread.Sleep(1000);
                Monitor.Exit(_syncObjForPrint);
            }
            
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void PrintStateWithFormat()
        {
            while (true)
            {
                Monitor.Enter(_syncObjForPrint);

                Console.WriteLine($"Current Formated State  {this.state} % from Thread  {Thread.CurrentThread.Name}");
                Thread.Sleep(1000);
                Monitor.Exit(_syncObjForPrint);
            }
        }
    }
    public class Singleton_Mutex
    {
        Mutex _mutexForUpdate = new Mutex();
        Mutex _mutexForPrint = new Mutex();
        object _syncObjForPrint = new object();
        int state;

        public static readonly Singleton_Mutex Instance = null;

        static Singleton_Mutex()
        {
            Instance = new Singleton_Mutex();
        }
        private Singleton_Mutex()
        {

        }

        
        public void Increment()
        {
            Console.WriteLine("Increment Started");
            Events.MRE.WaitOne();//Wait for Signal From Main

            this._mutexForUpdate.WaitOne();
                for (int i = 0; i < 5; i++)
                {
                    state++;
                    Console.WriteLine($"State incremented By {Thread.CurrentThread.Name}");
                    Thread.Sleep(2000);
            
                }
            this._mutexForUpdate.ReleaseMutex();
            Events.ARE1.Set();
            
            
            
        }
        //  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Decrement()
        {
            Console.WriteLine("Decrement Started");
            Events.MRE.WaitOne();//Wait for Signal From Main
            this._mutexForUpdate.WaitOne();
            for (int i = 0; i < 5; i++)
                {
                    state--;
                    Console.WriteLine($"State Decremented By {Thread.CurrentThread.Name}");
                    Thread.Sleep(5000);
                }
            this._mutexForUpdate.ReleaseMutex();
            Events.ARE2.Set();
        }

        //  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void PrintState()
        {
            Console.WriteLine("PrintState Started");
            Events.MRE.WaitOne();//Wait for Signal From Main

            while (true)
            {
                this._mutexForPrint.WaitOne();
                Console.WriteLine($"Current State {this.state} from Thread  {Thread.CurrentThread.Name}");
                Thread.Sleep(1000);
                this._mutexForPrint.ReleaseMutex();
            }

        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void PrintStateWithFormat()
        {
            Console.WriteLine("PrintStateWithFormat Started");
            Events.MRE.WaitOne();//Wait for Signal From Main
            while (true)
            {
                this._mutexForPrint.WaitOne();

                Console.WriteLine($"Current Formated State  {this.state} % from Thread  {Thread.CurrentThread.Name}");
                Thread.Sleep(1000);
                this._mutexForPrint.ReleaseMutex();
            }
        }
    }


    //M2->M1->M4->M3
    public class RelayTeam
    {
        public void Memeber1() {
        for(int i = 0; i < 10; i++)
            {

            }
        }
        public void Memeber2() {
        for(int i = 0; i < 20; i++) { }
        
        }
        public void Memeber3() {
            for (int i = 0; i < 15; i++) { }
        }
        public void Memeber4() {

            for (int i = 0; i < 25; i++) { }
        }

    }
}
