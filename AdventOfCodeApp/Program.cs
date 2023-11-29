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

            //var app = new AdventOfCode(client, 1);
            var app = new AdventOfCode(client, 2023, 1);

            //app.Run();
            app.RunTest();

        }
    }
}