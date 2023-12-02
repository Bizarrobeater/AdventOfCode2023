﻿using AdventOfCodeApp.DayClasses;
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

        public FileGetter FileGetter { get; private set; }
        private Stopwatch _stopwatch;
        private IDayLogic? _benchmarkLogic = null;


        public AdventOfCode(HttpClient httpClient, int day)
        {
            _stopwatch = new Stopwatch();
            FileGetter = new FileGetter(Year, day);
            HttpClient = httpClient;
            Day = day;
        }

        public AdventOfCode(HttpClient httpClient, int year, int day) : this(httpClient, day)
        {
            Year = year;
            FileGetter.Year = year;
        }

        public void RunTest(int questionNumber)
        {
            if (!(questionNumber == 1 || questionNumber == 2))
                throw new Exception("Question number is not possible, only 1 or 2");
            
            var files = FileGetter.GetFiles(true);
            var questionFiles = CreateQuestionDict(files, questionNumber);
            IDayLogic dayLogic = CreateDayLogic();

            Func<FileInfo, bool, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;
            long result;
            long expectedResult;
            foreach (KeyValuePair<int, FileInfo> questionFile in questionFiles)
            {
                result = Run(questionFunction, questionFile.Value);
                expectedResult = dayLogic.ExpectedTestResults[questionNumber][questionFile.Key];
                Console.WriteLine($"Expected result: {expectedResult}");
                Console.WriteLine($"Correct Result: {result == expectedResult}");
            }
        }

        public void RunActual(int questionNumber)
        {
            if (!(questionNumber == 1 || questionNumber == 2))
                throw new Exception("Question number is not possible, only 1 or 2");

            var files = FileGetter.GetFiles();

            IDayLogic dayLogic = CreateDayLogic();

            Func<FileInfo, bool, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;

            Run(questionFunction, files[0]);
        }

        public long RunActualBenchmarkTicks(int questionNumber)
        {
            if (!(questionNumber == 1 || questionNumber == 2))
                throw new Exception("Question number is not possible, only 1 or 2");

            var files = FileGetter.GetFiles(isBenchMark: true);
            IDayLogic dayLogic = _benchmarkLogic == null ? CreateDayLogic() : _benchmarkLogic;
            Func<FileInfo, bool, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;
            _stopwatch.Reset();
            _stopwatch.Start();

            questionFunction(files[0], true);
            _stopwatch.Stop();
            return _stopwatch.ElapsedTicks;
        }

        public long RunActualBenchmarkMilliseconds(int questionNumber)
        {
            if (!(questionNumber == 1 || questionNumber == 2))
                throw new Exception("Question number is not possible, only 1 or 2");
            
            var files = FileGetter.GetFiles(isBenchMark: true);
            IDayLogic dayLogic = _benchmarkLogic == null ? CreateDayLogic() : _benchmarkLogic;
            Func<FileInfo, bool, long> questionFunction = questionNumber == 1 ? dayLogic.RunQuestion1 : dayLogic.RunQuestion2;

            _stopwatch.Reset();
            _stopwatch.Start();

            questionFunction(files[0], true);
            _stopwatch.Stop();
            return _stopwatch.ElapsedMilliseconds;
        }

        private long Run(Func<FileInfo, bool, long> questionFunction, FileInfo file)
        {
            
            _stopwatch.Reset();
            _stopwatch.Start();

            long result = questionFunction(file, false);

            _stopwatch.Stop();

            Console.WriteLine($"Time taken in ms: {_stopwatch.ElapsedMilliseconds}\nResult: {result}");
            return result;
        }

        private Dictionary<int, FileInfo> CreateQuestionDict(List<FileInfo> files, int qNumber)
        {
            Dictionary<int, FileInfo> questionFiles = new Dictionary<int, FileInfo>();
            string cleanName;
            int testNumber;
            string[] splitName;
            foreach (FileInfo file in files)
            {
                cleanName = file.Name.Remove(file.Name.Length - file.Extension.Length);
                splitName = cleanName.Split('-');
                if (int.Parse(splitName[1]) != qNumber)
                    continue;
                
                testNumber = int.Parse(splitName[2]);

                questionFiles.Add(testNumber, file);
            }

            return questionFiles;
        }

        private IDayLogic CreateDayLogic()
        {
            IDayLogic? result = DayLogicFactory.CreateDayLogic(this);

            ArgumentNullException.ThrowIfNull(result);

            return result;
        }
        
    }
}
