using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    internal class SaveData
    {
        //Medeltemperatur ute och inne, per månad
        //Medelluftfuktighet inne och ute, per månad
        //Medelmögelrisk inne och ute, per månad.
        //Datum för höst och vinter 2016 (om något av detta inte inträffar, ange när det var som närmast)
        //Skriv ut algoritmen för mögel

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

        public static void ProcessListToSave(List<string> templist)
        {
            string filename = "";
            while (filename.Length < 3)
            { 
                filename = Helpers.GetStringFromUser("Enter filename at least three letters or digits: ");
            }
            SaveToTextFile(templist, filename + ".txt");
        }
    }
}
