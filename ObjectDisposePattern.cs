using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMgmt
{
    public static  class Signals
    {
        public static System.Threading.AutoResetEvent ResourceHandle = 
            new System.Threading.AutoResetEvent(false);
    }
    public enum ResourceState
    {
        BUSY,
        FREE
    }

    public static class Resource
    {
        public static ResourceState State { get; set; }
    }

    public class ResourceWrapper:IDisposable
    {
        bool isDisposed;
        public ResourceWrapper()
        {
            lock (Signals.ResourceHandle)
            {
                if (Resource.State == ResourceState.FREE)
                {
                    Resource.State = ResourceState.BUSY;
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId} ");
                }
                else
                {
                    Console.WriteLine($"Thread  {System.Threading.Thread.CurrentThread.ManagedThreadId} Awaiting For Resource Release ");
                    Signals.ResourceHandle.WaitOne();
                    Resource.State = ResourceState.BUSY;
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId} ");
                }
            }
        }
        public void UseResource()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Respurce Wrapper object Disposed") ;
            }
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Resource Used By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                System.Threading.Thread.Sleep(2000);
            }
        }
         void ReleaseResource()
        {
            Resource.State = ResourceState.FREE;
            Console.WriteLine($"Resource Released By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Signals.ResourceHandle.Set();
        }
        /* isDisposing to determine Destructor call from dispose or finalize*/
        protected void Dispose(bool isDisposing)
        {
            //Control - Dispose Code  Called Only Once
            if (!isDisposed)
            {
                if (isDisposing)
                {
                    isDisposed = true;
                    //Call from Dispose
                    GC.SuppressFinalize(this);
                    Console.WriteLine($"Resource Clean Up Done Using Dispose Method");
                    

                }
                else
                {
                    //Call From Finalize
                    Console.WriteLine($"Resource Clean Up Done Using Finalize Method");
                }
                ReleaseResource();
            }
            
        }
        public void Dispose()
        {
            Dispose(true);
        }

        ~ResourceWrapper()
        {

            Dispose(false);
        }
    }


    public class NewResourceWrapper : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    GC.SuppressFinalize(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~NewResourceWrapper()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Resource.State = ResourceState.FREE;
            new System.Threading.Thread(Client1).Start();
            new System.Threading.Thread(Client2).Start();
        }

        static void Client1()
        {
            ResourceWrapper _wrapper = new ResourceWrapper();
            try
            {
                _wrapper.UseResource();
            }
            finally
            {
                if (_wrapper is IDisposable)
                {
                    //_wrapper.Dispose();
                }
            }
            _wrapper.Dispose();
            _wrapper.Dispose();
            _wrapper.Dispose();
            _wrapper.Dispose();
            _wrapper = null;
            GC.Collect();
        }

        static void Client2()
        {
            //using (ResourceWrapper _wrapper = new ResourceWrapper())
            //{
            //    _wrapper.UseResource();
            //}
            //_wrapper.ReleaseResource();
            ResourceWrapper _wrapper = new ResourceWrapper();
            _wrapper.UseResource();
            _wrapper = null;
            GC.Collect();
        }
    }

}
