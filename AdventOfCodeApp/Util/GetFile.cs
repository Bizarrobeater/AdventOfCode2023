using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util
{
    internal class GetFile
    {
        public int Year { get; set; } = 2023;
        public int Day { get; set; }

        public GetFile(int day)
        {
            Day = day;
        }

        public GetFile(int year, int day) : this(day)
        {
            Year = year;
        }
    }
}
