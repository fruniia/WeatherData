using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Menu
    {
        internal static int MenuList(string header, int index, List<string> options, bool dayMenu)
        {
            ConsoleKeyInfo keyPressed;
            Console.CursorVisible = false;
            do
            {
                GUI.PrintMenu(header, 2, 1, index, options);
                keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.DownArrow && index != options.Count - 1) index++;
                else if (keyPressed.Key == ConsoleKey.UpArrow && index >= 1) index--;
                else if (keyPressed.Key == ConsoleKey.Escape) return -1;
                if (dayMenu)
                {
                    if (keyPressed.Key == ConsoleKey.RightArrow) return -2;
                    else if (keyPressed.Key == ConsoleKey.LeftArrow) return -3;
                }
            } while (keyPressed.Key != ConsoleKey.Enter);
            Console.CursorVisible = true;
            return index;

        }

        internal static DateTime SelectDateDay(List<SensorDataTime> sensorData)
        {
            int month = 6;
         
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(month, sensorData);          
            while (true)
            {
                Console.Clear();
                int dayIndex = Menu.MenuList(DateTimeFormatInfo.CurrentInfo.GetMonthName(month), 0, Helpers.FormatDates(dayList), true);
                if (dayIndex == -2 && month < 12) month++;
                else if (dayIndex == -3 && month > 6 && month > 1) month--;
                //else if (dayIndex == -1) return;
                else if (dayIndex >= 0) return (dayList[dayIndex]);
                dayList = Helpers.GetDatesPerMonthFromSensorData(month, sensorData);
            }
        }

        internal static DateTime SelectDateMonth()
        {
            List<string> monthList = DateTimeFormatInfo.CurrentInfo.MonthNames.Skip(5).SkipLast(1).ToList();
            while (true)
            {
                int monthIndex = Menu.MenuList("Pick a Month", 0, monthList, false);
                //else if (dayIndex == -1) return;
                if (monthIndex >= 0) return (new DateTime(2016, monthIndex + 6, 1));
            }
        }

        internal static void SelectDay(List<SensorDataTime> sensorData)
        {
            DateTime dateDay = SelectDateDay(sensorData);
            List<double> avgTemperaturePerDay = Helpers.GetTemperatureForSelectedDay(dateDay, sensorData);

            foreach (double temp in avgTemperaturePerDay)
            {
                Console.WriteLine(temp.ToString("0.00"));
            }
        }

        internal static void SelectMonth(List<SensorDataTime> sensorData)
        {
            DateTime date = SelectDateMonth();
            List<double> avgTemperaturePerMonth = GetTemperatureForSelectedMonth(date, sensorData);

            foreach (double temp in avgTemperaturePerMonth)
            {
                Console.WriteLine(temp.ToString("0.00"));
            }
        }

        private static List<double> GetTemperatureForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<double> avgTemperaturePerMonth = new();
            avgTemperaturePerMonth.Add(0.0);
            avgTemperaturePerMonth.Add(0.0);
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(date.Month, sensorData);

            for (int i = 0; i < dayList.Count; i++)
            {
                List<double> avgTemperaturePerDay = Helpers.GetTemperatureForSelectedDay(dayList[i], sensorData);

                avgTemperaturePerMonth[0] += (avgTemperaturePerDay[0]);
                avgTemperaturePerMonth[1] += (avgTemperaturePerDay[1]);
            }
            avgTemperaturePerMonth[0] = avgTemperaturePerMonth[0] / dayList.Count;
            avgTemperaturePerMonth[1] = avgTemperaturePerMonth[1] / dayList.Count;
            return avgTemperaturePerMonth;
        }

        private static List<double> GetHumidityForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<double> avgHumidityPerMonth = new();
            avgHumidityPerMonth.Add(0.0);
            avgHumidityPerMonth.Add(0.0);
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(date.Month, sensorData);

            for (int i = 0; i < dayList.Count; i++)
            {
                List<double> avgHumidityPerDay = Helpers.GetHumidityForSelectedDay(dayList[i], sensorData);

                avgHumidityPerMonth[0] += (avgHumidityPerDay[0]);
                avgHumidityPerMonth[1] += (avgHumidityPerDay[1]);
            }
            avgHumidityPerMonth[0] = avgHumidityPerMonth[0] / dayList.Count;
            avgHumidityPerMonth[1] = avgHumidityPerMonth[1] / dayList.Count;
            return avgHumidityPerMonth;
        }

        internal static void GetTemperatureForAllMonths(List<SensorDataTime> sensorData)
        {

            List<string> avgTemperaturePerMonthList = new();

            for (var i = 6; i <= 12; i++)
            {
                List<double> avgTemperaturePerMonth = GetTemperatureForSelectedMonth(new DateTime(2016, i, 1), sensorData);

                avgTemperaturePerMonthList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i).PadRight(10) + ":   Inside: " + avgTemperaturePerMonth[0].ToString("0.00") + "   Outside: " + avgTemperaturePerMonth[1].ToString("0.00"));
            }

            foreach (string temp in avgTemperaturePerMonthList)
            {
                Console.WriteLine(temp);
            }
        }

        internal static void GetHumidityForAllMonths(List<SensorDataTime> sensorData)
        {

            List<string> avgHumidityPerMonthList = new();

            for (var i = 6; i <= 12; i++)
            {
                List<double> avgHumidityPerMonth = GetHumidityForSelectedMonth(new DateTime(2016, i, 1), sensorData);

                avgHumidityPerMonthList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i).PadRight(10) + ":   Inside: " + avgHumidityPerMonth[0].ToString("0") + " %   Outside: " + avgHumidityPerMonth[1].ToString("0") + " %");
            }

            foreach (string temp in avgHumidityPerMonthList)
            {
                Console.WriteLine(temp);
            }
        }

        internal static List<SensorDataTime> GetAvgUnitPerDayList(List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> result = new();

            for (var i = 6; i <= 12; i++)
            {
                List<SensorDataTime> AvgUnitDataList = Helpers.GetUnitForSelectedMonth(new DateTime(2016, i, 1), sensorData);
                foreach (SensorDataTime unit in AvgUnitDataList) result.Add(unit);
            }

            //List<string> unitList = new List<string>();
            //for (int i = 0; i < result.Count; i++)
            //{
            //    unitList.Add(insideResult[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + insideResult[i].Location.PadRight(13) + "Temperature: " + insideResult[i].Temp.PadRight(10) + "Humidity: " + insideResult[i].Humidity + " %");
            //    unitList.Add(outsideResult[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + outsideResult[i].Location.PadRight(13) + "Temperature: " + outsideResult[i].Temp.PadRight(10) + "Humidity: " + outsideResult[i].Humidity + " %");
            //}

            //foreach (string data in unitList)
            //{
            //    Console.WriteLine(data);
            //}

            return result;
        }
    }
}
