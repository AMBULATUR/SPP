using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

namespace Spp3n4
{
    class Program
    {
        static Mutex mutex;
        static int debugInt;
        static void Main(string[] args)
        {
            OsHandle();
        }

        static void Mutex()
        {
            mutex = new Mutex();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                Task task = new Task(() => SomeWork());
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Completed without exception");
        }
        static void SomeWork()
        {
            //mutex.Lock();
            debugInt++;
            Console.WriteLine($"Some work in Thread {Thread.CurrentThread.ManagedThreadId}, debug int = {debugInt}");
            //mutex.Unlock();
        }

        static void OsHandle()
        {
            var b = new OSHandle();
            b.Dispose();
        }
        
    }
}
