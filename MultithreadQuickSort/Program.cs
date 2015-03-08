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
        private static StreamWriter output = new StreamWriter("output.txt");

        static void Main(string[] args)
        {
            MultithreadQSort.log = false;

            /* int[] array = new int[] { 10, 8, 23, 24, 35, 34, 24, 44, 25, 5, 6, -7, 15, 35, 23, 45, 35, 3, 
                446, 45, 49, 42, 65, 132, 34, 35, 325, 67, 33, 54, 23, 4, -21, -42, -34234, -42, -243 };

            int[] array1 = new int[] { 10, 8, 23, 24, 35, 34, 24, 44, 25, 5, 6, -7, 15, 35, 23, 45, 35 }; */

            /* Array.ForEach(array, (x) => { Console.Write("{0} ", x); });
            Console.WriteLine();
            Array.ForEach(array1, (x) => { Console.Write("{0} ", x); });
            Console.WriteLine(); */

            Random rnd = new Random();

            List<int[]> arrays = new List<int[]>();
            List<int[]> arraysB = new List<int[]>();


            for (int j = 0; j < 10; j++)
            {
                arrays.Add(new int[500000]);
                arraysB.Add(new int[500000]);

                for (int i = 0; i < arrays[j].Length; i++)
                {
                    arrays[j][i] = rnd.Next(1000000);
                    arraysB[j][i] = arrays[j][i];
                }
            }

            Console.WriteLine("Multithread Sorts start");

            DateTime start = DateTime.Now;

            for (int j = 0; j < 10; j++)
                MultithreadQSort.QuickSort(arrays[j]);

            DateTime finish = DateTime.Now;
            Console.WriteLine("Multithread Sorts finish. Time: {0} s", (finish - start).TotalSeconds);

            for (int j = 0; j < 10; j++)
            {
                output.WriteLine("Array {0}", j);
                Array.ForEach(arrays[j], (x) => { output.Write("{0} ", x); });
                output.WriteLine();
                output.WriteLine();
            }

            Console.WriteLine("Not-multithread Sorts start");
            start = DateTime.Now;
            for (int j = 0; j < 10; j++)
                QSort(arraysB[j]);
            finish = DateTime.Now;
            Console.WriteLine("Multithread Sorts finish. Time: {0} s", (finish - start).TotalSeconds); 

            /*
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
                MultithreadQSort.QuickSort(array);
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

            */

            output.Flush();
            output.Close();

            Console.ReadKey();
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
