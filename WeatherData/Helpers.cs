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

        internal static List<SensorDataTime> GetSelectedDateWithRegex(string pattern, List<SensorDataTime> sensorData)
        {
           return RegexHelper.GetMatchValue(pattern, sensorData);
           
        }

        internal static List<double> GetDivideDataTempPerLocation(List<SensorDataTime> sensorData, string pattern)
        {
            List<SensorDataTime> insideData = new();
            List<SensorDataTime> outsideData = new();
            foreach (SensorDataTime data in sensorData)
            {
                if (data.Location == ("Inside"))

                    insideData.Add(data);

                else
                    outsideData.Add(data);
            }
            return GetAverageTemperature(insideData, outsideData, pattern);
        }

        internal static List<double> GetAverageTemperature(List<SensorDataTime> insideData, List<SensorDataTime> outsideData, string dataPattern)
        {
            Regex pattern = new Regex(dataPattern);
            List<double> temps = new List<double>();
            temps.Add(GetAverageTemperatureFromModel(pattern, insideData));
            temps.Add(GetAverageTemperatureFromModel(pattern, outsideData));
            return temps;
        }

        internal static double GetAverageTemperatureFromModel(Regex pattern, List<SensorDataTime> listData)
        {
            double temp = 0.0;
            foreach (SensorDataTime data in listData)
            {               
                    temp += double.Parse(data.Temp, System.Globalization.CultureInfo.InvariantCulture);
            }
            return temp / (double)listData.Count;
        }

        internal static string ReturnFormattedDate(string date)
        {
            if (date.Length == 1) return "0" + date;
            return date;
        }

        internal static List<DateTime> GetDatesPerMonthFromSensorData(int month, List<SensorDataTime> sensorData, int year = 2016)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var data in sensorData.Select(x => x.Date).Distinct())
            {
                if (data.Date.Year == year && data.Date.Month == month)
                {
                    //data.Date.Day
                    dates.Add(data.Date);
                }
            }
            return dates;
        }

    }
}
