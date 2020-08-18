 class Window : System.Windows.Forms.Form
    {
        System.Windows.Forms.Button _btn = new System.Windows.Forms.Button();
        System.Threading.SynchronizationContext _mainContext;
        public Window()
        {
            _mainContext = System.Threading.SynchronizationContext.Current;
            _btn.Click += _btn_Click;
            _btn.Text = "Click";
            this.Controls.Add(_btn);
        }

        private async void _btn_Click(object sender, EventArgs e)
        {
            //State1
            Console.WriteLine($"Statement 1 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 2 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 3 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //Async Call - State 2
            await Task.Run(() => {

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Async Statement {i} Executed By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    System.Threading.Thread.Sleep(500);
                }

            });
            _mainContext.Send(new System.Threading.SendOrPostCallback((obj) =>
            {

                Console.WriteLine($"Statement 4 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 5 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 6 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 7 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            }
            ), null);

            //State 3
            //Async Call - state 4
            string result = await Task<string>.Run(() => {

                for (int i = 5; i < 10; i++)
                {
                    Console.WriteLine($"Async Statement {i} Executed By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    System.Threading.Thread.Sleep(500);
                }
                return "Episode 5 Result";
            });
            //State5
            Console.WriteLine($"{result}");
            Console.WriteLine($"Statement 8 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 9 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 10 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 11 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");

        }
    }
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {


            Algorithm();
            Console.WriteLine($"Statement N Executed By{System.Threading.Thread.CurrentThread.ManagedThreadId} ");
            
            while (true) { }

            //    System.Windows.Forms.Application.Run(new Window());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
        }

        static async void  Algorithm()
        {
            //State1
            Console.WriteLine($"Statement 1 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 2 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 3 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //Async Call - State 2
            await Task.Run(() => {

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Async Statement {i} Executed By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    System.Threading.Thread.Sleep(500);
                }
            
            });
            //mainThreadContext.Send(new System.Threading.SendOrPostCallback((obj) =>
            //{

                Console.WriteLine($"Statement 4 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 5 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 6 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Statement 7 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            //}
            //),null);
       
            //State 3
            //Async Call - state 4
            string result=await Task<string>.Run(() => {

                for (int i = 5; i < 10; i++)
                {
                    Console.WriteLine($"Async Statement {i} Executed By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    System.Threading.Thread.Sleep(500);
                }
                return "Episode 5 Result";
            });
            //State5
            Console.WriteLine($"{result}");
            Console.WriteLine($"Statement 8 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 9 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 10 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Statement 11 Exceucuted By {System.Threading.Thread.CurrentThread.ManagedThreadId}");


        }
    }
