using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    internal class ReadData
    {
        private static List<string> ReadDataFromTextFile(string file)
        { 
            List<string> temporaryFileList = new();
            if (File.Exists(file))
                temporaryFileList = File.ReadLines(file).ToList();
            else
                GUI.PrintText("Couldn't read file.");

            return temporaryFileList;
        }

        public static List<string> GetFileDate()
        {
            List<string> files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Data").ToList();
            List<string> filenames = new();
            for (int i = 0; i < files.Count; i++)
            {
                filenames.Add(Path.GetFileName(files[i]));
            }
            int index = Menu.MenuList("Choose file", 0, filenames);

            return ReadDataFromTextFile(files[index]);
        }
    }
}
