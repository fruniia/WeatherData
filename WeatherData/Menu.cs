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

            //List<DateTime> dayList = Helpers.GetDatesPerMonth(month);
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(month, sensorData);
            dayList.ForEach(x => Console.WriteLine(x));
            Console.ReadLine();
            //while (true)
            //{
            //    int dayIndex = Menu.MenuList("Pick a Date", 0, Helpers.FormatDates(dayList), true);
            //    if (dayIndex == -2 && month < 12) month++;
            //    else if (dayIndex == -3 && month > 6 && month > 1) month--;
            //    else if (dayIndex == -1) return;
            //    else if (dayIndex >= 0) return (dayList[dayIndex]);
            //    dayList = Helpers.GetDatesPerMonth(month);
            //}
            return DateTime.Today;

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

        internal static void SelectDay()
        {
            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();
            DateTime dateDay = SelectDateDay(sensorData);
            string pattern = RegexHelper.GetPattern(Helpers.ReturnFormattedDate(dateDay.Month.ToString()), Helpers.ReturnFormattedDate(dateDay.Day.ToString()));
            List<string> matches = Helpers.GetSelectedDateWithRegex(pattern);
            List<double> temperaturePerDay = Helpers.GetDivideDataTempPerLocation(matches, pattern);

            foreach (double temp in temperaturePerDay)
            {
                Console.WriteLine(temp.ToString("0.00"));
            }

        }

        internal static void SelectMonth()
        {
            DateTime date = SelectDateMonth();
            List<double> avgTemperaturePerMonth = new();
            avgTemperaturePerMonth.Add(0.0);
            avgTemperaturePerMonth.Add(0.0);
            int insideCounting = 0;
            int outsideCounting = 0;

            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                string pattern = RegexHelper.GetPattern(Helpers.ReturnFormattedDate(date.Month.ToString()), Helpers.ReturnFormattedDate(i.ToString()));
                List<string> matches = Helpers.GetSelectedDateWithRegex(pattern);
                List<double> avgTemperaturePerDay = Helpers.GetDivideDataTempPerLocation(matches, pattern);

                if (avgTemperaturePerDay[0] !> 0)
                {
                    avgTemperaturePerMonth[0] += (avgTemperaturePerDay[0]);
                    insideCounting++;
                }

                if (avgTemperaturePerDay[1] !> 0)
                {
                    avgTemperaturePerMonth[1] += (avgTemperaturePerDay[1]);
                    outsideCounting++;
                }
            }
            avgTemperaturePerMonth[0] = avgTemperaturePerMonth[0] / insideCounting;
            avgTemperaturePerMonth[1] = avgTemperaturePerMonth[1] / outsideCounting;

            foreach(double temp in avgTemperaturePerMonth)
            {
                Console.WriteLine(temp.ToString("0.00"));
            }
        }
    }
}
