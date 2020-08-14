using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;//TPL

namespace TPLDemo
{
    class Program
    {
        static void Main_old(string[] args)
        {
            Task _task1 = new Task(Task1); // new Task(new Action(Program.Task1))
            _task1.Start();

            _task1.Wait();

            Task<string> _task2 = new Task<string>(AppendTask);
            _task2.Start();
            _task2.Wait();

            Console.WriteLine(_task2.Result);//Non-Blocking Call


        }

        static void Task1()
        {
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Task1 - ThreadPoolThread: {System.Threading.Thread.CurrentThread.IsThreadPoolThread}");

            }

        }
       static string AppendTask()
        {
            string temp = default(string);
            for(int i = 0; i < 10; i++)
            {
                temp += i.ToString();
            }
            return temp;
        }


        static void Main_new()
        {
            CancellationTokenSource _tokenSource = new CancellationTokenSource();
            CancellationToken _token = _tokenSource.Token;
          

            
            Task _task = new Task(RunTask,_token,_token);
            _task.Start();//if(isCompleted)

            Thread.Sleep(5000);
           
            _tokenSource.Cancel();
            Console.WriteLine("Cancellation request Raised");
            Console.ReadLine();

        }

        static void RunTask(object state)
        {
            var cancellationToken = (CancellationToken)state;
            //Blocked Call
            //cancellationToken.Register(() => {

            //    Console.WriteLine("Cancelltion Requested");

            //});
            for (int i = 0; i < 10; i++)
            {
                //cancellationToken.ThrowIfCancellationRequested();
                    
                    Console.WriteLine($"Async Task {i}");
                if (cancellationToken.WaitHandle.WaitOne(1000))
                {
                    //Post Cancellation Action
                    return;
                }


            }
        }


        static void Main_Parent_Child()
        {

            Task homeLoanSubmittionTask = new Task(() => {

                Console.WriteLine("Home Loan Sumbition Task Begin");
                Task  _propertyVerificationTask = new Task(() => {

                    Console.WriteLine("Property Verification Task Begin");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine("Property Verification......in Progress");
                        Thread.Sleep(2000);
                    }
                    Console.WriteLine("Property Verification Task End");


                }, TaskCreationOptions.AttachedToParent);
                
                Task _elligibilityVerificationTask = new Task(() => {
                   
                    Task _cbilVerificationTask = new Task(() => {

                        Console.WriteLine("CBIL Verification Task Begin");
                        for (int i = 0; i < 5; i++)
                        {
                            if (i == 3)
                            {
                                throw new InvalidOperationException("Negetive CIBIL Score Found");
                            }
                            Console.WriteLine("CBIL Verification......in Progress");
                            Thread.Sleep(3000);
                        }
                        Console.WriteLine("CBIL Verification Task End");
                    }, TaskCreationOptions.AttachedToParent);
                    Task _incomeTaxVerificationTask = new Task(() => {

                        Console.WriteLine("Income tax Verification Task Begin");
                        for (int i = 0; i < 5; i++)
                        {
                            if (i == 4)
                            {
                                throw new InvalidOperationException("Tax Deafulter");
                            }
                            Console.WriteLine("Income Tax Verification......in Progress");
                            Thread.Sleep(5000);
                        }
                        Console.WriteLine("Income Verification Task End");

                    }, TaskCreationOptions.AttachedToParent);

                    _cbilVerificationTask.Start();
                    _incomeTaxVerificationTask.Start();

                }, TaskCreationOptions.AttachedToParent);

                _propertyVerificationTask.Start();
                _elligibilityVerificationTask.Start();

            });

            homeLoanSubmittionTask.Start();

            try
            {
                homeLoanSubmittionTask.Wait();
            }
            catch (AggregateException ex)
            {

                var exceptions= ex.Flatten();
                
            }

        }

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Press Any Key To Send Ajax Request");
                Console.ReadKey();
                SendAjaxRequest();
            }
           
        }

        static void SendAjaxRequest()
        {
            Task<int> _requestTask = new Task<int>(() => {
                Console.WriteLine("New Http Request Sent");
                Thread.Sleep(1000);
                for(int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Awaiting For Reply");
                    Thread.Sleep(1000);
                }
                Random _random = new Random();
                int number=_random.Next(0,9);
                Console.WriteLine($"Received  Reply  {number}");
                return number;
            });

          Task  responseProcessingTask=  _requestTask.ContinueWith((pt) => {

              
              
              for (int i = 0; i < 5; i++)
              {
                  Console.WriteLine("Processing The Response........");
                  Thread.Sleep(1000);
              }
              int number = pt.Result;
              if (number % 2 != 0)
              {
                  throw new Exception("Http 404 Resource Not Found ");
              }

          });

            responseProcessingTask.ContinueWith((pt) => {

                Console.WriteLine("Logging Task Begin");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Upating Log.....{pt.Exception.InnerException.Message}");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Logging Task End");


            }, TaskContinuationOptions.OnlyOnFaulted);

            responseProcessingTask.ContinueWith((pt) => {


                Console.WriteLine("DataConvertion Task Begin");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Converting Data.......");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("DataConvertion task Ended");

            }, TaskContinuationOptions.NotOnFaulted|TaskContinuationOptions.OnlyOnRanToCompletion);
            _requestTask.Start();
        }
    }
}
