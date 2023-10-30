using System;
using System.Threading.Tasks;

namespace NumberOfMultipliesAsync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tasks = InitTasksPool(Environment.ProcessorCount, 1_000_000_000, 2_000_000_000);

            foreach (var task in tasks) { task.Start(); }
            Task.WaitAll(tasks);
            
            int count = 0;
            foreach (var task in tasks) { count += task.Result; }

            Console.WriteLine(count);
        }

        private static Task<int>[] InitTasksPool(int numOfTasks, int numFrom, int numTo)
        {
            Task<int>[] tasks = new Task<int>[numOfTasks];
            int interval = (numTo - numFrom) / numOfTasks;
            for (int i = 0; i < numOfTasks; i++)
            {
                numTo = numFrom + interval;
                int from = numFrom;
                int to = numTo;
                tasks[i] = new Task<int>(() => IntervalCalc(from, to));
                numFrom = numTo;
            }
            return tasks;
        }

        private static int IntervalCalc(int numFrom, int numTo)
        {
            int innerCount = 0;
            for (int i = numFrom; i < numTo; i++)
            {
                int sum = Sum(i);
                bool isDivided = IsDivide(sum, i % 10);
                if (isDivided) { innerCount++; };
            }
            return innerCount;
        }

        private static bool IsDivide(int num, int div)
        {
            if (div == 0) { return false; }
            if (num % div == 0) { return true; }
            else { return false; }
        }

        private static int Sum(int value)
        {
            int sum = 0;
            while (value > 0)
            {
                sum += value % 10;
                value /= 10;
            }
            return sum;
        }
    }
}