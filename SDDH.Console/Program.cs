using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDDH.Consoles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //DoManager();
            ParallelDemo();
            //ForTaskDemo();
        }

        /// <summary>
        /// 性能优于for循环，建议使用此方式
        /// </summary>
        private static void ParallelDemo()
        {
            List<int> list = new List<int>();
            for (int i = 0; i <= 1000; i++)
            {
                list.Add(i);
            }
            Console.WriteLine("Mainthread start! threadId:" + Thread.CurrentThread.ManagedThreadId);
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 5;//最大任务并行数
            Parallel.ForEach<int>(list, options, o => TaskProcess(o));
            Console.WriteLine("Mainthread end! threadId:" + Thread.CurrentThread.ManagedThreadId);
            Console.Read();
        }

        private static void TaskProcess(int index)
        {
            currentThreads++;
            string log = string.Format("task log:{0},subthreadId:{1}", index, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(50);
            Console.WriteLine(log);
        }

        private static int maxThreads = 5; //最大线程数
        private static int currentThreads = 0; //当前线程数
        /// <summary>
        /// 此种方式开启多线程传参有问题，待优化，勿用
        /// </summary>
        private static void ForTaskDemo()
        {
            List<int> demoList = new List<int>();
            for (int i = 0; i <= 1000; i++)
            {
                demoList.Add(i);
            }
            Console.WriteLine(string.Format("main start, threadId is: {0}, currentThreads = {1}", Thread.CurrentThread.ManagedThreadId, currentThreads));
            for (int i = 0; i < demoList.Count; i++)
            {
                if (currentThreads < maxThreads)
                {
                    Console.WriteLine(string.Format("index:<{0}>,currentThreads:{1}", i, currentThreads));
                    Task task = Task.Run(() =>
                    {
                        //TaskProcess2(i);
                        string log = string.Format("+++task log:【{0}】,subthreadId:{1},currentThreads:{2}+++", i, Thread.CurrentThread.ManagedThreadId, currentThreads);
                        Console.WriteLine(log);
                        //currentThreads++; //计数有问题，待优化优化
                        Interlocked.Increment(ref currentThreads);
                    });
                    task.ContinueWith(o =>
                    {
                        string log = string.Format("---finished :{0},subthreadId:{1}, currentThreads:{2}---", o, Thread.CurrentThread.ManagedThreadId, currentThreads);
                        Console.WriteLine(log);
                        //currentThreads--;
                        Interlocked.Decrement(ref currentThreads);
                    });
                }
                else
                {
                    Console.WriteLine(string.Format("main else, threadId is: {0}, currentThreads = {1}>>>>>>>>>>>>>>>>", Thread.CurrentThread.ManagedThreadId, currentThreads));
                    i--;
                }
            }
            Console.WriteLine(string.Format("main end, threadId is: {0}, currentThreads = {1}", Thread.CurrentThread.ManagedThreadId, currentThreads));
            Console.Read();
        }

        private static void TaskProcess2(int index)
        {
            string log = string.Format("+++task log:{0},subthreadId:{1},currentThreads:{2}+++", index, Thread.CurrentThread.ManagedThreadId, currentThreads);
            Thread.Sleep(50);
            Console.WriteLine(log);
        }


        private static void DoManager()
        {

            Console.WriteLine("Start!");
            JobManager.RunDemo().Wait();
            Console.Read();
        }

    }
}
