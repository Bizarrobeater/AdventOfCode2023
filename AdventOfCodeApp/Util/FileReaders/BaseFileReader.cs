using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal abstract class BaseFileReader<T>
    {

        private string ReadFile(FileInfo file)
        {
            string result = "";
            using (StreamReader sr = file.OpenText())
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        public T GetReadableFileContent(FileInfo file)
        {
            string contentResult = ReadFile(file);
            return ConvertFileContentToReadable(contentResult);
        }

        protected abstract T ConvertFileContentToReadable(string content);
    }
}
