using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CpuLoadTest
{
    // Legacy cpu load test. Use in the case GpuLoadTest throws cause of hybridizer dependencies.
    class CpuLoadTest
    {
        // Maximum number of threads that can be used. Dont set much higher than what your device has
        public static readonly int MAX_PARALELLISM = 64;
        // Operations cuantity to be resolved. Setting this higher will make the test take more time. Known max: 68000000
        public static readonly int TEST_COUNT = 68000000;
        private static double[] a = new double[TEST_COUNT], b = new double[TEST_COUNT];

        static void Main(string[] args)
        {
            Console.WriteLine("Preparing data for the test . . .");
            //var executionWatch = Stopwatch.StartNew();

            PrepareData();

            //executionWatch.Stop();
            //Console.WriteLine("The time elapsed during the prepare stage is " + Math.Floor((double)executionWatch.ElapsedMilliseconds / 1000) + " s " + executionWatch.ElapsedMilliseconds % 1000 + " ms.");

            Console.WriteLine("Press any key to launch the test.");
            Console.ReadKey(); Console.WriteLine("");

            RunOnCPU(a, b);

            Console.ReadKey();
        }

        internal static void PrepareData()
        {
            int MsgFreq = TEST_COUNT / 5;
            Random generator = new Random();
            Parallel.For(
                0,
                TEST_COUNT - 1,
                new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALELLISM * 12 },
                i =>
                {
                    a[i] = generator.Next(int.MaxValue);
                    Thread.Sleep((int)a[i] % 3);
                    b[i] = generator.Next(int.MaxValue);
                    if ((i + 1) % MsgFreq == 0)
                        Console.WriteLine("\tIteration " + (i + 1));
                });
        }

        internal static void RunOnCPU(double[] a, double[] b)
        {
            Console.WriteLine("["+ DateTime.Now.ToLongTimeString() + "] Started heavy load task on CPU.");
            var executionWatch = Stopwatch.StartNew();

            Run(a, b);

            executionWatch.Stop();
            Console.WriteLine("["+ DateTime.Now.ToLongTimeString() + "] Finished heavy load task.");
            double elapsedS = Math.Floor((double)executionWatch.ElapsedMilliseconds / 1000), elapsedMs = executionWatch.ElapsedMilliseconds % 1000;
            Console.WriteLine("The time elapsed during the load is "+elapsedS+" s "+elapsedMs+" ms.");
        }

        internal static void Run(double[] a, double[] b)
        {
            Parallel.For(
                0,
                TEST_COUNT - 1,
                new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALELLISM },
                i => {
                    double auxA = a[i];
                    a[i] += Math.Atan2(Math.Log10(b[i]), Math.Log10(a[i]));
                    b[i] = Math.Atan2(Math.Log10(auxA), Math.Log10(b[i]));
                });
        }
    }
}
