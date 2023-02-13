﻿using System;
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
                            meteroDays.Add(outsideDataDate[counting - 5]);
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
            result.Add("Meteorologist Autumn Date: " + dateTimes[0].ToString("yyyy.MM.dd"));
            result.Add("Meteorologist Winter Date: " + dateTimes[1].ToString("yyyy.MM.dd"));
            return result;
        }

        internal static List<SensorDataTime> GetUnitForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(date.Month, sensorData);
            List<SensorDataTime> unitAvgPerDay = new();

            for (int i = 0; i < dayList.Count; i++)
            {
                unitAvgPerDay.Add(GetUnitAvgForSelectedDay(dayList[i], sensorData));
            }
            return unitAvgPerDay;
        }

        private static SensorDataTime GetUnitAvgForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = Helpers.GetSelectedDateData(dateDay, sensorData);

            double temperature = 0.0;
            double humidity = 0.0;
            double moldRisk = 0.0;
            foreach (SensorDataTime data in matches)
            {

                double temporaryTemp = double.Parse(data.Temp, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryTemp > -30 && temporaryTemp < 40)
                    temperature += temporaryTemp;

                double temporaryHumidity = double.Parse(data.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryHumidity > 0 && temporaryHumidity < 100)
                    humidity += temporaryHumidity;

            }
            temperature = temperature / (double)matches.Count;
            humidity = humidity / (double)matches.Count;
            moldRisk = moldRisk / (double)matches.Count;

            SensorDataTime dataDay = new SensorDataTime
            {
                Date = dateDay,
                Temp = temperature.ToString("0.00"),
                Humidity = humidity.ToString("0"),
                MoldRisk = moldRisk.ToString("0"),
                Location = sensorData[0].Location,
            };

            return dataDay;
        }

        internal static List<SensorDataTime> DivideDataPerLocation(List<SensorDataTime> sensorData, string location)
        {
            List<SensorDataTime> locationData = new();

            foreach (SensorDataTime data in sensorData)
                if (data.Location == (location))
                    locationData.Add(data);

            return locationData;
        }

        internal static List<SensorDataTime> GetSortedListPerTemp(List<SensorDataTime> dataAvgPerDay)
        {
            var names = dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.Temp, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
            return names;
        }

        internal static List<SensorDataTime> GetSortedListPerTempReverse(List<SensorDataTime> dataAvgPerDay)
        {
            var names = dataAvgPerDay
                .OrderBy(g => double.Parse(g.Temp, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
            return names;
        }

        internal static List<SensorDataTime> GetSortedListPerHumidity(List<SensorDataTime> dataAvgPerDay)
        {
            var names = dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.Humidity, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
            return names;
        }

        internal static List<SensorDataTime> GetSortedListPerHumidityReverse(List<SensorDataTime> dataAvgPerDay)
        {
            var names = dataAvgPerDay
                .OrderBy(g => double.Parse(g.Humidity, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
            return names;
        }

        internal static List<SensorDataTime> GetSortedListPerMold(List<SensorDataTime> dataAvgPerDay)
        {
            var names = dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.MoldRisk, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).ToList();
            return names;
        }

        internal static List<string> ConvertModelListToStringListWithMolding(List<SensorDataTime> modelList)
        {
            List<string> unitList = new List<string>();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(modelList[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %   Mold Risk: " + modelList[i].MoldRisk + " %");
            return unitList;
        }

        internal static List<string> ConvertModelListToStringList(List<SensorDataTime> modelList)
        {
            List<string> unitList = new List<string>();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(modelList[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %");
            return unitList;
        }

        internal static List<string> ConvertModelListToStringListWithMoldingPerYear(List<SensorDataTime> modelList)
        {
            List<string> unitList = new List<string>();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i+6).PadRight(10) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %   Mold Risk: " + modelList[i].MoldRisk + " %");
            return unitList;
        }

        internal static string ConvertDoubleToStringListWithDate(List<double> avgTemperature)
        {
            return "  Inside: " + avgTemperature[0].ToString("0.00") + "  Outside: " + avgTemperature[1].ToString("0.00");
        }
    }
}
