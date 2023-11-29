using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp
{
    internal class AdventOfCode
    {
        public int Year { get; private set; } = 2023;
        public int Day { get; private set; }
        public HttpClient HttpClient { get; private set; }

        public FileGetter FileGetter => new FileGetter(Year, Day);

        public AdventOfCode(HttpClient httpClient, int day)
        {
            HttpClient = httpClient;
            Day = day;
        }

        public AdventOfCode(HttpClient httpClient, int year, int day) : this(httpClient, day)
        {
            Year = year;
        }

        public void RunTest(int questionNumber)
        {
            var files = FileGetter.GetFiles(true);
            IDayLogic dayLogic = DayLogicFactory.CreateDayLogic(this);
            Stopwatch stopwatch = new Stopwatch();

            Func<FileInfo, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;
            long result;
            foreach (var file in files)
            {
                result = Run(questionFunction, file);
                Console.WriteLine($"Expected result: {dayLogic});
            }

        }

        public void RunActual(int questionNumber)
        {
            var files = FileGetter.GetFiles();

            IDayLogic dayLogic = DayLogicFactory.CreateDayLogic(this);

            Func<FileInfo, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;

            Run(questionFunction, files[0]);
        }

        private long Run(Func<FileInfo, long> questionFunction, FileInfo file)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            long result = questionFunction(file);

            stopwatch.Stop();

            Console.WriteLine($"Time taken in ms: {stopwatch.ElapsedMilliseconds}\nResult: {result}");
            return result;
        }
        
    }
}
