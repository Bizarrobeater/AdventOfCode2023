using AdventOfCodeApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp
{
    internal class AdventOfCode
    {
        public int Year { get; private set; } = 2023;
        public int Day { get; private set; }
        public HttpClient HttpClient { get; private set; }

        public GetFile FileGetter => new GetFile(Year, Day);

        public AdventOfCode(HttpClient httpClient, int day)
        {
            HttpClient = httpClient;
            Day = day;
        }

        public AdventOfCode(HttpClient httpClient, int year, int day) : this(httpClient, day)
        {
            Year = year;
        }

        public void RunTest()
        {
            var files = FileGetter.GetFiles(true);

            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }

        }

        public void Run()
        {
            var files = FileGetter.GetFiles();

            foreach (var item in files)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
