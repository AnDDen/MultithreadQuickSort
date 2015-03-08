using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace MultithreadQuickSort
{
    class Program
    {
        //private static StreamWriter input = new StreamWriter("input.txt");
        //private static StreamWriter output = new StreamWriter("output.txt");

        static void Main(string[] args)
        {
            //int[] array = new int[] { 10, 8, 23, 24, 35, 34, 24, 44, 25, 5, 6, -7, 15, 35, 23, 45, 35, 3, 446, 45, 49, 42, 65, 132, 34 };

            Random rnd = new Random();

            for (int j = 0; j < 10; j++)
            {
                int[] array = new int[500000];
                int[] arrayB = new int[500000];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = rnd.Next(1000000);
                    arrayB[i] = array[i];
                }

                Console.WriteLine("[Array {0}] multithread sort start", j + 1);
                DateTime start = DateTime.Now;
                QuickSort(array);
                DateTime finish = DateTime.Now;
                Console.WriteLine("[Array {0}] multithread sort finish", j + 1);

                Console.WriteLine("[Array {0}] multithread sort time: {1} s", j + 1, (finish - start).TotalSeconds);

                Console.WriteLine("[Array {0}] non-multithread sort start", j + 1);
                start = DateTime.Now;
                QSort(arrayB);
                finish = DateTime.Now;
                Console.WriteLine("[Array {0}] non-multithread sort finish", j + 1);

                Console.WriteLine("[Array {0}] non-multithread sort time: {1} s", j + 1, (finish - start).TotalSeconds);
                Console.WriteLine();
            }

            //output.Flush();
            //output.Close();

            Console.ReadKey();
        }

        private static Semaphore pool;
        private static int t = 0, count = 0;

        private static bool log = false;

        public static void QuickSort(int[] a)
        {
            pool = new Semaphore(5, 5);

            count = 1;
            ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, 0, a.Length - 1 });

            while (count != 0) ;

            //if (log) Console.WriteLine("Sort complete");

        }

        private static void QuickSort(object state)
        {
            //int num = t++;
            //if (log) Console.WriteLine("[Thread {0}]\tEnters thread pool and waits for the semaphore.", num);

            pool.WaitOne();

            //if (log) Console.WriteLine("[Thread {0}]\tEnters the semaphore.", num);
       
            //extracting arguments
            object[] array = state as object[];
            int[] a = array[0] as int[];
            int p = Convert.ToInt32(array[1]);
            int r = Convert.ToInt32(array[2]);

            //Console.WriteLine("[Thread {0}]\tp = {1}; r = {2}", num, p, r);

            if (p < r)
            {
                int q = Partition(a, p, r);
                //Console.WriteLine("[Thread {0}]\tq = {1}", num, q);
                count += 2;
                ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, p, q - 1 });
                ThreadPool.QueueUserWorkItem(new WaitCallback(QuickSort), new object[] { a, q + 1, r });
            }

            int k = pool.Release();
            //if (log) Console.WriteLine("[Thread {0}]\tReleases the semaphore. Previous semaphore count: {1}", num, k);

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

        // сортировка без потоков
        public static void QSort(int[] a)
        {
            qsort(a, 0, a.Length - 1);
        }

        private static void qsort(int[] a, int p, int r)
        {
            if (p < r)
            {
                int q = Partition(a, p, r);
                qsort(a, p, q - 1);
                qsort(a, q + 1, r);
            }
        }
    }
}
