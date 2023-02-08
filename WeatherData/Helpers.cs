using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Data;

namespace WeatherData
{
    internal class Helpers
    {
        public static string GetStringFromUser(string prompt)
        {
            Console.Write(prompt);
            string? result = Console.ReadLine();
            result = result.Any(x => char.IsLetterOrDigit(x)).ToString();
            if (result == null)
            {
                result = "";
            }
            return result;
        }
        internal static List<DateTime> GetDates(int month, int year = 2016)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .ToList();
        }
        internal static List<string> FormatDates(List<DateTime> dates)
        {
            List<string> formattedDates = new();
            for (int i = 0; i < dates.Count; i++)
            {
                formattedDates.Add(dates[i].DayOfWeek.ToString().PadRight(10) + ": " + dates[i].ToString("yyyy/MM/dd"));
            }
            return formattedDates;
        }

        //Utomhus
        //Inomhus
        //Sparad data

        //Textfil
        //Medeltemperatur ute och inne, per månad
        //Medelluftfuktighet inne och ute, per månad
        //Medelmögelrisk inne och ute, per månad.
        //Datum för höst och vinter 2016 (om något av detta inte inträffar, ange när det var som närmast)
        //Skriv ut algoritmen för mögel

        //Följande information ska kunna visas
        /*  Utomhus
         *  ◦ Medeltemperatur per dag, för valt datum (sökmöjlighet med validering)
            ◦ Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
            ◦ Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
            ◦ Sortering av minst till störst risk av mögel
            ◦ Datum för meteorologisk Höst
            ◦ Datum för meteologisk vinter (OBS Mild vinter!)
             Inomhus
            ◦ Medeltemperatur för valt datum (sökmöjlighet med validering)
            ◦ Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
            ◦ Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
            ◦ Sortering av minst till störst risk av mögel
                     */

        internal static void CompareSelectedDateWithRegex(DateTime date)
        {
            string pattern = RegexHelper.GetPattern(ReturnFormattedDate(date.Month.ToString()), ReturnFormattedDate(date.Day.ToString()));

            List<string> sensorData = ReadData.GetSensorData();
            List<string> matches = RegexHelper.GetMatchValue(pattern, sensorData);
            DivideDataPerLocation(matches, pattern);
        }
        internal static void DivideDataPerLocation(List<string> sensorData, string pattern)
        {
            List<string> insideData = new();
            List<string> outsideData = new();
            foreach (string data in sensorData)
            {
                if (data.Contains("Inne"))

                    insideData.Add(data);

                else
                    outsideData.Add(data);
            }
            AverageTemperature(insideData, outsideData, pattern);
        }

        internal static void AverageTemperature(List<string> insideData, List<string> outsideData, string dataPattern)
        {
            Regex pattern = new Regex(dataPattern);
            double insideAvgTemp = 0.0;
            double outsideAvgTemp = 0.0;
            foreach (var data in insideData)
            {
                Match match = pattern.Match(data);
                if (match.Success)
                {                    
                    insideAvgTemp += double.Parse((match.Groups["Temp"].Value).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            insideAvgTemp = insideAvgTemp / (double)insideData.Count;

            foreach (var data in outsideData)
            {
                Match match = pattern.Match(data);
                if (match.Success)
                {
                    outsideAvgTemp += double.Parse((match.Groups["Temp"].Value).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }

            }
            outsideAvgTemp = outsideAvgTemp / (double)outsideData.Count;
            Console.WriteLine($"Inside {insideAvgTemp.ToString("0.00")} Outside {outsideAvgTemp.ToString("0.00")}");
            Console.ReadLine();
        }



        internal static string ReturnFormattedDate(string date)
        {
            if (date.Length == 1) return "0" + date;
            return date;
        }
        //internal static List<string> GetSelectedData(string regex)
        //{ 

        //}
    }
}
