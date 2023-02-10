using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    internal class SaveData
    {
        

        private static void SaveToTextFile(List<string> templist, string filename)
        {
            string output = string.Empty;
            try
            {
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "Data\\" + filename, templist);
                output = $"Successfully saved data. Filename {filename}";
            }
            catch(Exception e)
            { 
                output = "Failed to save data\n\n" + e;
            }
            GUI.PrintText(output);
        }

        internal static void ProcessListToSave(List<string> templist)
        {
            string filename = "";
            while (filename.Length < 3)
            { 
                filename = Helpers.GetStringFromUser("Enter filename at least three letters or digits: ");
            }
            SaveToTextFile(templist, filename + ".txt");
        }

        internal static void SaveListToFile(List<string> templist, string fileName)
        {
            SaveToTextFile(templist, fileName + ".txt");
        }
    }
}
