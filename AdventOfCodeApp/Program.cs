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

            //var app = new AdventOfCode(client, 2023, 1);
            var app = new AdventOfCode(client, 2023, 2);

            //app.RunActual(1);
            //app.RunTest(1);
            //app.RunTest(2);
            app.RunActual(2);

            //Benchmark(app, 2);
            //Console.ReadKey();
        }

        public static void Benchmark(AdventOfCode app, int question)
        {
            int runs = 1_000;
            List<long> timeTaken = new List<long>();

            for (int i = 0; i < runs; i++)
            {
                timeTaken.Add(app.RunActualBenchmark(question));
            }
            timeTaken.Sort();

            Console.WriteLine("Benchmark:");
            Console.WriteLine($"First Run Time: {timeTaken[0]}");
            Console.WriteLine($"Last Run Time: {timeTaken[timeTaken.Count - 1]}");
            Console.WriteLine($"Average: {timeTaken.Average()}");
            Console.WriteLine($"Median: {timeTaken[timeTaken.Count / 2]}");
            Console.WriteLine($"Max Time: {timeTaken.Max()}");
            Console.WriteLine($"Min Time: {timeTaken.Min()}");

            Dictionary<long, int> resultAmounts = new Dictionary<long, int>();

            foreach ( long amount in timeTaken )
            {
                if (!resultAmounts.ContainsKey( amount ) ) 
                {
                    resultAmounts[amount] = 0;
                }
                resultAmounts[amount]++;
            }
            Console.WriteLine("Result counts:");
            foreach(KeyValuePair<long, int> pair in resultAmounts)
            {
                Console.WriteLine($"Time taken - {pair.Key}, Count - {pair.Value}");
            }
        }
    }
}