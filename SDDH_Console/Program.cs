using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDDH.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TaskDemo();
        }

        public static void TaskDemo()
        {
            Console.WriteLine("MainThreadStart,ThreadId:" + Thread.CurrentThread.ManagedThreadId);
            int taskCount = 0;
            List<Task> tasks = new List<Task>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 10; i++)
            {
                Task task = Task.Run(() =>
                {
                    Console.WriteLine("SubThreadId:" + Thread.CurrentThread.ManagedThreadId);
                    Interlocked.Increment(ref taskCount);
                });

                //Task task1 = Task.Run<int>(() =>
                //{
                //    Interlocked.Increment(ref taskCount);
                //    return i;
                //});
            }
            Task.WhenAll(tasks).ContinueWith(o =>
            {
                watch.Stop();
                long usedTime = watch.ElapsedMilliseconds;
            });
            Console.WriteLine("MainThreadEnd,ThreadId:" + Thread.CurrentThread.ManagedThreadId);
        }

    }
}
