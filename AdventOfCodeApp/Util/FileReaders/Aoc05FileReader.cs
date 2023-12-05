using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class Aoc05FileReader : BaseFileReader<string[]>
    {
        protected override string[] ConvertFileContentToReadable(string content)
        {
            string doubleNewLine = Environment.NewLine + Environment.NewLine;
            return content.Split(doubleNewLine);
        }
    }
}
