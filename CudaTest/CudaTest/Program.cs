using Hybridizer.Runtime.CUDAImports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CudaTest
{
    class Program
    {
        // Maximum number of threads that can be used. Dont set much higher than what your device has
        // Recommended up to 128 for cpu or up to the cuda cores of your graphics card
        public static readonly int MAX_PARALELLISM = 2000;
        // Operations cuantity to be resolved. Setting this higher will make the test take more time
        public static readonly int TEST_COUNT = 2000000;

        [EntryPoint]
        public static void Run(double[] a, double[] b)
        {
            Parallel.For(
                0,
                a.Length-1,
                new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALELLISM},
                i => {
                    double auxA = a[i];
                    a[i] += Math.Atan( Math.Log10(a[i]) + Math.Log10(b[i]) );
                    b[i] = Math.Atan2(Math.Log10(auxA), Math.Log10(b[i]));
                });
        }

        static void Main(string[] args)
        {
            double[] a = new double[TEST_COUNT], b = new double[TEST_COUNT];
            Random generator = new Random();
            for (int i = 0; i < TEST_COUNT; i++)
            {
                a[i] = generator.Next(int.MaxValue);
                Thread.Sleep(3);
                b[i] = generator.Next(int.MaxValue);
            }

            Console.Out.WriteLine("Should the program run on the GPU? (y/n)");

            if (Console.ReadKey().Equals('y'))
                RunOnGPU(a, b);
            else
                RunOnCPU(a, b);

            Console.ReadKey();
        }

        internal static void RunOnGPU(double[] a, double[] b)
        {
            cudaDeviceProp prop;
            cuda.GetDeviceProperties(out prop, 0);
            //if .SetDistrib is not used, the default is .SetDistrib(prop.multiProcessorCount * 16, 128)
            HybRunner runner = HybRunner.Cuda();

            // create a wrapper object to call GPU methods instead of C#
            dynamic wrapped = runner.Wrap(new Program());

            Console.Out.WriteLine(String.Format("[{0}] Started heavy load task on GPU."), DateTime.Now.ToString());

            wrapped.Run(a, b);

            Console.Out.WriteLine(String.Format("[{0}] Finished heavy load task."), DateTime.Now.ToString());
        }

        internal static void RunOnCPU(double[] a, double[] b)
        {
            Console.Out.WriteLine(String.Format("[{0}] Started heavy load task on CPU."), DateTime.Now.ToString());

            Run(a, b);

            Console.Out.WriteLine(String.Format("[{0}] Finished heavy load task."), DateTime.Now.ToString());
        }

    }

}
