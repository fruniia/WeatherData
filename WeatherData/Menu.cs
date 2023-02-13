using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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

        private static DateTime SelectDateDay(List<SensorDataTime> sensorData)
        {
            int month = 6;
         
            List<DateTime> dayList = Helpers.GetDatesPerMonthFromSensorData(month, sensorData);          
            while (true)
            {
                Console.Clear();
                int dayIndex = Menu.MenuList(DateTimeFormatInfo.CurrentInfo.GetMonthName(month), 0, Helpers.FormatDates(dayList), true);
                if (dayIndex == -2 && month < 12) month++;
                else if (dayIndex == -3 && month > 6 && month > 1) month--;
                else if (dayIndex >= 0) return (dayList[dayIndex]);
                dayList = Helpers.GetDatesPerMonthFromSensorData(month, sensorData);
            }
        }

        private static DateTime SelectDateMonth()
        {
            List<string> monthList = DateTimeFormatInfo.CurrentInfo.MonthNames.Skip(5).SkipLast(1).ToList();
            while (true)
            {
                int monthIndex = Menu.MenuList("Pick a Month", 0, monthList, false);
                if (monthIndex >= 0) return (new DateTime(2016, monthIndex + 6, 1));
            }
        }

        private static void SelectDay(List<SensorDataTime> sensorData)
        {
            Console.Clear();
            DateTime dateDay = SelectDateDay(sensorData);
            List<double> avgTemperaturePerDay = Helpers.GetTemperatureForSelectedDay(dateDay, sensorData);

            Console.Clear();
            GUI.PrintText("Selected Day AvgTemperature\n\n" + dateDay.ToString("yyyy.MM.dd") + Helpers.ConvertDoubleToStringListWithDate(avgTemperaturePerDay));
            Console.ReadLine();
            Console.Clear();
        }

        private static void SelectMonth(List<SensorDataTime> sensorData)
        {
            Console.Clear();
            DateTime date = SelectDateMonth();
            List<double> avgTemperaturePerMonth = Helpers.GetTemperatureForSelectedMonth(date, sensorData);

            Console.Clear();
            GUI.PrintText("Selected Month AvgTemperature\n\n" + date.ToString("MMMM") + Helpers.ConvertDoubleToStringListWithDate(avgTemperaturePerMonth));
            Console.ReadLine();
            Console.Clear();
        }

        private static List<string> ShowTemperatureForAllMonths(List<SensorDataTime> sensorData)
        {

            List<string> avgTemperaturePerMonthList = new();

            for (var i = 6; i <= 12; i++)
            {
                List<double> avgTemperaturePerMonth = Helpers.GetTemperatureForSelectedMonth(new DateTime(2016, i, 1), sensorData);

                avgTemperaturePerMonthList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i).PadRight(10) + ":   Inside: " + avgTemperaturePerMonth[0].ToString("0.00") + "   Outside: " + avgTemperaturePerMonth[1].ToString("0.00"));
            }

            return avgTemperaturePerMonthList;
        }

        private static void ShowMoldPerDay(List<SensorDataTime> insideDataAvgPerDay, List<SensorDataTime> outsideDataAvgPerDay)
        {
            Console.Clear();
            List<SensorDataTime> insideMoldPerDay = MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerDay);
            List<SensorDataTime> outsideMoldPerDay = MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerDay);
            GUI.PrintList("Mold Inside Per Day: ((Temperatur * 2) * ((Humidity - 75) * 4))", Helpers.ConvertModelListToStringListWithMolding(insideMoldPerDay), true, 1, 2);
            GUI.PrintList("Mold Outside Per Day: ((Temperatur * 2) * ((Humidity - 75) * 4))", Helpers.ConvertModelListToStringListWithMolding(outsideMoldPerDay), true, 95, 2);
            Console.ReadLine();
            Console.Clear();
        }

        private static void ShowMoldPerMonth(List<SensorDataTime> insideDataAvgPerMonth, List<SensorDataTime> outsideDataAvgPerMonth)
        {
            Console.Clear();
            GUI.PrintList("Mold Inside Per Month: ((Temperatur * 2) * ((Humidity - 75) * 4)", Helpers.ConvertModelListToStringListWithMoldingPerYear(MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerMonth)), true, 1, 2);
            GUI.PrintList("Mold Outside Per Month: ((Temperatur * 2) * ((Humidity - 75) * 4)", Helpers.ConvertModelListToStringListWithMoldingPerYear(MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerMonth)), true, 95, 2);
            Console.ReadLine();
            Console.Clear();
        }

        private static void ShowSortedData(List<SensorDataTime> insideDataAvgPerDay, List<SensorDataTime> outsideDataAvgPerDay)
        {
            Console.Clear();
            List<SensorDataTime> sortedInsideDataPerTemp = Helpers.GetSortedListPerTemp(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerTemp = Helpers.GetSortedListPerTemp(outsideDataAvgPerDay);

            List<SensorDataTime> sortedInsideDataPerHumidity = Helpers.GetSortedListPerHumidity(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerHumidity = Helpers.GetSortedListPerHumidity(outsideDataAvgPerDay);


            List<SensorDataTime> sortedInsideDataPerTempReverse = Helpers.GetSortedListPerTempReverse(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerTempReverse = Helpers.GetSortedListPerTempReverse(outsideDataAvgPerDay);

            List<SensorDataTime> sortedInsideDataPerHumidityReverse = Helpers.GetSortedListPerHumidityReverse(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerHumidityReverse = Helpers.GetSortedListPerHumidityReverse(outsideDataAvgPerDay);


            GUI.PrintList("sortedInsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedInsideDataPerTemp), true, 1, 2);
            GUI.PrintList("sortedOutsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedOutsideDataPerTemp), true, 82, 2);
            GUI.PrintList("sortedInsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedInsideDataPerHumidity), true, 1, 10);
            GUI.PrintList("sortedOutsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedOutsideDataPerHumidity), true, 82, 10);

            GUI.PrintList("sortedInsideDataPerTempReverse", Helpers.ConvertModelListToStringList(sortedInsideDataPerTempReverse), true, 1, 18);
            GUI.PrintList("sortedOutsideDataPerTempReverse", Helpers.ConvertModelListToStringList(sortedOutsideDataPerTempReverse), true, 82, 18);
            GUI.PrintList("sortedInsideDataPerHumidityReverse", Helpers.ConvertModelListToStringList(sortedInsideDataPerHumidityReverse), true, 1, 26);
            GUI.PrintList("sortedOutsideDataPerHumidityReverse", Helpers.ConvertModelListToStringList(sortedOutsideDataPerHumidityReverse), true, 82, 26);

            Console.ReadLine();
            Console.Clear();
        }

        private static void ShowSortedMoldData(List<SensorDataTime> insideDataAvgPerDay, List<SensorDataTime> outsideDataAvgPerDay)
        {
            Console.Clear();
            List<SensorDataTime> sortedInsideDataPerMold = Helpers.GetSortedListPerMold(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerMold = Helpers.GetSortedListPerMold(outsideDataAvgPerDay);
            GUI.PrintList("sortedInsideDataPerTemp", Helpers.ConvertModelListToStringListWithMolding(sortedInsideDataPerMold), true, 1, 2);
            GUI.PrintList("sortedOutsideDataPerTemp", Helpers.ConvertModelListToStringListWithMolding(sortedOutsideDataPerMold), true, 95, 2);

            Console.ReadLine();
            Console.Clear();
        }

        private static void SaveDataFiles(List<SensorDataTime> sensorData, List<SensorDataTime> insideDataAvgPerMonth, List<SensorDataTime> outsideDataAvgPerMonth)
        {
            SaveData.SaveListToFile(ShowTemperatureForAllMonths(sensorData), "TemperatureForAllMonths");
            SaveData.SaveListToFile(Helpers.GetHumidityForAllMonths(sensorData), "HumidityForAllMonths");
            SaveData.SaveListToFile(Helpers.ConvertMeteoroligicalToStringList(Helpers.GetMeteorological(sensorData, 10)), "MeteorologicalData");
            SaveData.SaveListToFile(Helpers.ConvertModelListToStringListWithMoldingPerYear(MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerMonth)), "RiskForMoldGrowthPerMonthInside");
            SaveData.SaveListToFile(Helpers.ConvertModelListToStringListWithMoldingPerYear(MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerMonth)), "RiskForMoldGrowthPerMonthsOutside");

            GUI.PrintText("All Data Is Saved!");
            Console.ReadLine();
            Console.Clear();
        }

        private static void ShowSavedFiles()
        {
            Console.Clear();
            List<string> dataFiles = ReadData.GetSelectedData();
            GUI.PrintList("", dataFiles, false, 2, 3);
            Console.ReadLine();
            Console.Clear();
        }

        internal static void MainMenu(List<SensorDataTime> sensorData, List<SensorDataTime> insideDataAvgPerDay, List<SensorDataTime> outsideDataAvgPerDay, List<SensorDataTime> insideDataAvgPerMonth, List<SensorDataTime> outsideDataAvgPerMonth)
        {
            List<string> mainMenuText = new()
            {
                "Select Day",
                "Select Month",
                "Show MoldRisk Per Day",
                "Show MoldRisk Per Month",
                "Show Sorted Data",
                "Show Sorted MoldRisk Data",
                "Save Data Files",
                "Show Data Files",
                "Exit"
            };

            while (true)
            {
                int index = MenuList("Main Menu", 0, mainMenuText, false);

                switch (index)
                {
                    case 0:
                        SelectDay(sensorData);
                        break;
                    case 1:
                        SelectMonth(sensorData);
                        break;
                    case 2:
                        ShowMoldPerDay(insideDataAvgPerDay, outsideDataAvgPerDay);
                        break;
                    case 3:
                        ShowMoldPerMonth(insideDataAvgPerMonth, outsideDataAvgPerMonth);
                        break;
                    case 4:
                        ShowSortedData(insideDataAvgPerDay, outsideDataAvgPerDay);
                        break;
                    case 5:
                        ShowSortedMoldData(MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerDay), MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerDay));
                        break;
                    case 6:
                        SaveDataFiles(sensorData, MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerMonth), MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerMonth));
                        break;
                    case 7:
                        ShowSavedFiles();
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
