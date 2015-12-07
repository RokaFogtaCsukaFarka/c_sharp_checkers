using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfx3nr
{
    public class Logger
    {
        public Boolean loggerOn = false;
        public string fullFilePath = Directory.GetCurrentDirectory() + @"\logger.txt";

        public Logger(Boolean loggerState)
        {
            using(StreamWriter file = File.CreateText(fullFilePath))
                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            this.loggerOn = loggerState;
        }

        public void Write(string str)
        {
            using (var file = File.AppendText(fullFilePath))
                file.WriteLine(str);
        }

    }
}
