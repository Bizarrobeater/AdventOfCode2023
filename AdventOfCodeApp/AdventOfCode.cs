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

        public string FileName => $"Adv{Year}{Day.ToString("00")}";

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
            var files = GetFiles(true);

            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }

        }

        public void Run()
        {
            var files = GetFiles();

            foreach (var item in files)
            {
                Console.WriteLine(item.Name);
            }
        }

        public List<FileInfo> GetFiles(bool isTest = false)
        {
            var fileDir = GetFileFolder();
            if (!fileDir.Exists)
                throw new Exception("Some error regarding file folders");

            Func<FileInfo, bool> fileFunction = isTest ? IsTestFile : IsActualFile;

            List<FileInfo> result = new List<FileInfo>();

            foreach (FileInfo file in fileDir.GetFiles() )
            {
                if (fileFunction(file))
                {
                    result.Add(file);
                }
            }
            return result;
        }

        public bool IsTestFile(FileInfo file)
        {
            return file.Name.StartsWith($"{FileName}-");
        }

        public bool IsActualFile(FileInfo file)
        {
            return file.Name == FileName;
        }

        private DirectoryInfo GetFileFolder()
        {
            string target = "InputFiles";
            string currDirString = Directory.GetCurrentDirectory();
            DirectoryInfo currDir = new DirectoryInfo(currDirString);
            bool isBaseFolder = false;
            DirectoryInfo[] currDirContains;
            DirectoryInfo foundDir = new DirectoryInfo(target);

            while (!isBaseFolder && currDir.Exists)
            {
                currDirContains = currDir.GetDirectories();
                foreach (var dir in currDirContains)
                {
                    if (dir.Name == target)
                    {
                        foundDir = dir;
                        isBaseFolder = true;
                        break;
                    }
                    
                }
                currDir = currDir.Parent ?? new DirectoryInfo(target);
            }
            return foundDir;
        }
    }
}
