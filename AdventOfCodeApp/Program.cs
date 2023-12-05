using AdventOfCodeApp.DayClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AdventOfCodeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddHttpClient();

            IHost host = builder.Build();
            
            var client = host.Services.GetRequiredService<HttpClient>();

            var app = new AdventOfCode();
            //var app = new AdventOfCode(2023, 1);
            //var app = new AdventOfCode(2023, 2);
            //var app = new AdventOfCode(2023, 3);
            //var app = new AdventOfCode(2023, 4);

            //app.RunTest(1);
            //app.RunActual(1);

            //app.RunTest(2);
            //app.RunActual(2);

            //Benchmark(app, 2);
            Benchmark(app, 2, ticks: true);
            //Console.ReadKey();
        }

        public static void Benchmark(AdventOfCode app, int question, bool ticks = false)
        {
            int runs = 100_000;
            List<long> timeTaken = new List<long>();
            Dictionary<long, int> resultAmounts = new Dictionary<long, int>();
            long time;
            Func<int, long> benchmarkFunction = ticks ? app.RunActualBenchmarkTicks : app.RunActualBenchmarkMilliseconds;

            for (int i = 0; i < runs; i++)
            {
                time = benchmarkFunction(question);
                timeTaken.Add(time);
                if (!resultAmounts.ContainsKey(time))
                    resultAmounts[time] = 0;
                resultAmounts[time]++;
            }
            timeTaken.Sort();
            string explainText = ticks ? "in ticks" : "in milliseconds";
            Console.WriteLine($"Benchmark {explainText}:");
            Console.WriteLine($"First Run Time: {timeTaken[0]}");
            Console.WriteLine($"Last Run Time: {timeTaken[timeTaken.Count - 1]}");
            Console.WriteLine($"Average: {timeTaken.Average()}");
            Console.WriteLine($"Median: {timeTaken[timeTaken.Count / 2]}");
            Console.WriteLine($"Max Time: {timeTaken.Max()}");
            Console.WriteLine($"Min Time: {timeTaken.Min()}");

            if (ticks)
                return;

            Console.WriteLine("Result counts:");
            List<long> uniqueTimes = resultAmounts.Keys.ToList();
            uniqueTimes.Sort();
            foreach(long uniqueTime in uniqueTimes)
            {
                Console.WriteLine($"Time taken - {uniqueTime}, Count - {resultAmounts[uniqueTime]}");
            }
        }
    }
}