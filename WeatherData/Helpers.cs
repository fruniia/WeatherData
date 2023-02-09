using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Data;
using WeatherData.Models;

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

        internal static List<DateTime> GetDatesPerMonth(int month, int year = 2016)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .ToList();
        }

        internal static List<DateTime> GetDatesPerYear(int v)
        {
            throw new NotImplementedException();
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

        internal static List<string> GetSelectedDateWithRegex(string pattern)
        {
            List<string> sensorData = ReadData.GetSensorData();
            List<string> matches = RegexHelper.GetMatchValue(pattern, sensorData);
            return matches;
        }

        internal static List<double> GetDivideDataTempPerLocation(List<string> sensorData, string pattern)
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
            return GetAverageTemperature(insideData, outsideData, pattern);
        }

        internal static List<double> GetAverageTemperature(List<string> insideData, List<string> outsideData, string dataPattern)
        {
            Regex pattern = new Regex(dataPattern);
            List<double> temps = new List<double>();
            temps.Add(GetAverageTemperatureFromList(pattern, insideData));
            temps.Add(GetAverageTemperatureFromList(pattern, outsideData));
            return temps;
        }

        internal static double GetAverageTemperatureFromList(Regex pattern, List<string> listData)
        {
            double temp = 0.0;
            foreach (string data in listData)
            {
                Match match = pattern.Match(data);
                if (match.Success)
                    temp += double.Parse((match.Groups["Temp"].Value).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            return temp / (double)listData.Count;
        }

        internal static string ReturnFormattedDate(string date)
        {
            if (date.Length == 1) return "0" + date;
            return date;
        }
        //internal static List<DateTime> GetDatesPerMonth(int month, int year = 2016)
        //{
        //    return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
        //                     .Select(day => new DateTime(year, month, day))
        //                     .ToList();
        //}

        internal static List<DateTime> GetDatesPerMonthFromSensorData(int month, List<SensorDataTime> sensorData, int year = 2016)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var data in sensorData)
            {
                if(data.Date.Year == year && data.Date.Month == month)
                dates.Add(data.Date);
            }
            return dates;
        }

    }
}
