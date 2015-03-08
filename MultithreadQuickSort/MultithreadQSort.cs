using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultithreadQuickSort
{
    public class MultithreadQSort
    {
        public static bool log = true;

        private static Semaphore pool;
        private static int t = 0, count = 0;

        public static void QuickSort(int[] a)
        {
            pool = new Semaphore(8, 16);

            count = 1;
            ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, 0, a.Length - 1 });

            while (count != 0) ;

            if (log) Console.WriteLine("Sort complete");

        }

        private static void QuickSort(object state)
        {
            int num = t++;
            if (log) Console.WriteLine("[Thread {0}]\tEnters thread pool and waits for the semaphore.", Thread.CurrentThread.ManagedThreadId);

            pool.WaitOne();

            if (log) Console.WriteLine("[Thread {0}]\tEnters the semaphore.", Thread.CurrentThread.ManagedThreadId);

            //extracting arguments
            object[] array = state as object[];
            int[] a = array[0] as int[];
            int p = Convert.ToInt32(array[1]);
            int r = Convert.ToInt32(array[2]);

            if (p < r)
            {
                int q = Partition(a, p, r);
                count += 2;
                ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, p, q - 1 });
                ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, q + 1, r });
            }

            int k = pool.Release();
            if (log) Console.WriteLine("[Thread {0}]\tReleases the semaphore. Previous semaphore count: {1}", Thread.CurrentThread.ManagedThreadId, k);

            count--;
        }

        private static int Partition(int[] a, int p, int r)
        {
            int t;
            int i = p - 1;
            int x = a[r];

            for (int j = p; j < r; j++)
            {
                if (a[j] <= x)
                {
                    t = a[++i];
                    a[i] = a[j];
                    a[j] = t;
                }
            }

            t = a[i + 1];
            a[i + 1] = a[r];
            a[r] = t;
            return i + 1;
        }
    }
}
