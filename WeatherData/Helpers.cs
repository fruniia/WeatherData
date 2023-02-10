using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
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
                result = "";
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
        //------- Medeltemperatur ute och inne, per månad
        //------- Medelluftfuktighet inne och ute, per månad
        //Medelmögelrisk inne och ute, per månad.
        //------- Datum för höst och vinter 2016 (om något av detta inte inträffar, ange när det var som närmast)
        //Skriv ut algoritmen för mögel

        //Följande information ska kunna visas
        /*  Utomhus
         *  ------ ◦ Medeltemperatur per dag, för valt datum (sökmöjlighet med validering)
            ◦ Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
            ◦ Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
            ◦ Sortering av minst till störst risk av mögel
            ------ ◦ Datum för meteorologisk Höst
            ------ ◦ Datum för meteologisk vinter (OBS Mild vinter!)
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

        internal static List<double> GetDivideDataTempPerLocation(List<SensorDataTime> sensorData)
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
            return GetAverageTemperature(insideData, outsideData);
        }

        internal static List<double> GetDivideDataHumidityPerLocation(List<SensorDataTime> sensorData)
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
            return GetAverageHumidity(insideData, outsideData);
        }

        internal static List<double> GetAverageTemperature(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {
            
            List<double> temps = new();
            temps.Add(GetAverageTemperatureFromModel(18, 30, insideData));
            temps.Add(GetAverageTemperatureFromModel(-30, 36, outsideData));
            return temps;
        }

        internal static List<double> GetAverageHumidity(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {

            List<double> temps = new();
            temps.Add(GetAverageHumidityFromModel(0, 100, insideData));
            temps.Add(GetAverageHumidityFromModel(0, 100, outsideData));
            return temps;
        }

        internal static double GetAverageTemperatureFromModel(double minTemp, double maxTemp, List<SensorDataTime> listData)
        {
            double temp = 0.0;
            foreach (SensorDataTime data in listData)
            {
                double temporaryTemp = double.Parse(data.Temp, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryTemp > minTemp && temporaryTemp < maxTemp)
                    temp += temporaryTemp;
            }
            return temp / (double)listData.Count;
        }

        internal static double GetAverageHumidityFromModel(double minHumidity, double maxHumidity, List<SensorDataTime> listData)
        {
            double humidity = 0.0;
            foreach (SensorDataTime data in listData)
            {
                //Console.WriteLine(data.Humidity);
                //Console.ReadLine();


                double temporaryHumidity = double.Parse(data.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryHumidity > minHumidity && temporaryHumidity < maxHumidity)
                    humidity += temporaryHumidity;

            }
            return humidity / (double)listData.Count;
        }

        internal static string ReturnFormattedDate(string date)
        {
            if (date.Length == 1) return "0" + date;
            return date;
        }

        internal static List<DateTime> GetDatesPerMonthFromSensorData(int month, List<SensorDataTime> sensorData, int year = 2016)
        {
            List<DateTime> dates = new();
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

        internal static List<SensorDataTime> GetSelectedDateData(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = new();
            foreach (var data in sensorData)
                if (data.Date.Year == dateDay.Year && data.Date.Month == dateDay.Month && data.Date.Day == dateDay.Day)
                    matches.Add(data);
            return matches;
        }

        internal static List<SensorDataTime> GetSelectedDivideDataTemp(List<SensorDataTime> sensorData, string selectedLocation)
        {
            List<SensorDataTime> locationData = new();
            foreach (SensorDataTime data in sensorData)
                if (data.Location == selectedLocation)
                    locationData.Add(data);
            return locationData;
        }

        internal static List<double> GetTemperatureForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = Helpers.GetSelectedDateData(dateDay, sensorData);
            return Helpers.GetDivideDataTempPerLocation(matches);
        }

        internal static List<double> GetHumidityForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = Helpers.GetSelectedDateData(dateDay, sensorData);
            return Helpers.GetDivideDataHumidityPerLocation(matches);
        }

        internal static List<DateTime> GetMeteorological(List<SensorDataTime> sensorData, int minValue)
        {
            List<double> outsideDataTemp = new();
            List<DateTime> outsideDataDate = new();
            foreach (DateTime date in sensorData.Select(x => x.Date).Distinct())
            {
                List<double> DataTemp = GetTemperatureForSelectedDay(date, sensorData);
                outsideDataDate.Add(date);

                outsideDataTemp.Add(DataTemp[1]);
            }

            List<DateTime> meteroDays = new();
            double temproraryAverage = 5.0;

            for (int i = 0; i < outsideDataTemp.Count - 5; i++)
            {
                int counting = i;

                List<double> doubles = new();
                while (true)
                {
                    if (counting == i + 5)
                        break;

                    if (outsideDataTemp[counting] <= minValue)
                        doubles.Add(outsideDataTemp[counting]);

                    if (outsideDataDate[counting].Date.AddDays(1) != outsideDataDate[counting + 1].Date && counting != i + 4) 
                        break;

                    counting++;
                }

                if (doubles.Count == 5)
                {
                    if (meteroDays.Count == 0)
                    {
                        if (doubles.Average() <= minValue)
                        {
                            meteroDays.Add(outsideDataDate[counting]);
                            meteroDays.Add(outsideDataDate[counting]);
                        }
                    }

                    else if (temproraryAverage > doubles.Average())
                    {
                        temproraryAverage = doubles.Average();
                        meteroDays[1] = outsideDataDate[counting - 5];
                    }
                }
            }
            return meteroDays;
        }

        internal static List<string> ConvertMeteoroligicalToStringList(List<DateTime> dateTimes)
        {
            List<string> result = new List<string>();
            result.Add("Meteorologist Autumn Date: " + dateTimes[0]);
            result.Add("Meteorologist Winter Date: " + dateTimes[1]);
            return result;
        }

    }
}
