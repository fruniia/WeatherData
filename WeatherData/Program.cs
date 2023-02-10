using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();


            //List<string> mainMenuText = new()
            //{
            //    "Login",
            //    "Create User",
            //    "Exit"
            //};
            //mainMenuText = ReadData.GetSensorData();

            //Menu.MenuList("Test", 1, mainMenuText, false);




            //SaveData.SaveListToFile(Helpers.ConvertMeteoroligicalToStringList(Helpers.GetMeteorological(sensorData, 10)), "MeteorologicalData");

            //Menu.SelectDay(sensorData);
            //Menu.SelectMonth(sensorData);
            //Menu.GetTemperatureForAllMonths(sensorData);
            //Menu.GetHumidityForAllMonths(sensorData);

            List<SensorDataTime> insideDataAvgPerDay = Menu.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Inside"));
            List<SensorDataTime> outsideDataAvgPerDay = Menu.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Outside"));
            
            List<SensorDataTime> insideMold = MoldAlgorithm.CalculateRiskForMoldGrowth(insideDataAvgPerDay);
            List<SensorDataTime> outsideMold = MoldAlgorithm.CalculateRiskForMoldGrowth(outsideDataAvgPerDay);
            GUI.PrintList("sortedInsideDataPerTemp", Helpers.ConvertModelListToStringListWithMolding(insideMold), true, 1, 2);
            GUI.PrintList("sortedOutsideDataPerTemp", Helpers.ConvertModelListToStringListWithMolding(outsideMold), true, 95, 2);
            Console.ReadLine();

            //GUI.PrintList("Hej", Helpers.ConvertModelListToStringList(insideDataAvgPerDay), true, 1, 2);
            //GUI.PrintList("Hej", Helpers.ConvertModelListToStringList(outsideDataAvgPerDay), true, 85, 2);


            List<SensorDataTime> sortedInsideDataPerTemp = Helpers.GetSortedListPerTemp(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerTemp = Helpers.GetSortedListPerTemp(outsideDataAvgPerDay);

            List<SensorDataTime> sortedInsideDataPerHumidity = Helpers.GetSortedListPerHumidity(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerHumidity = Helpers.GetSortedListPerHumidity(outsideDataAvgPerDay);


            List<SensorDataTime> sortedInsideDataPerTempReverse = Helpers.GetSortedListPerTempReverse(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerTempReverse = Helpers.GetSortedListPerTempReverse(outsideDataAvgPerDay);

            List<SensorDataTime> sortedInsideDataPerHumidityReverse = Helpers.GetSortedListPerHumidityReverse(insideDataAvgPerDay);
            List<SensorDataTime> sortedOutsideDataPerHumidityReverse = Helpers.GetSortedListPerHumidityReverse(outsideDataAvgPerDay);


            GUI.PrintList("sortedInsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedInsideDataPerTemp), true, 1, 2);
            GUI.PrintList("sortedOutsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedOutsideDataPerTemp), true, 85, 2);
            GUI.PrintList("sortedInsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedInsideDataPerHumidity), true, 1, 10);
            GUI.PrintList("sortedOutsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedOutsideDataPerHumidity), true, 85, 10);

            Console.ReadLine();
            Console.Clear();

            GUI.PrintList("sortedInsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedInsideDataPerTempReverse), true, 1, 2);
            GUI.PrintList("sortedOutsideDataPerTemp", Helpers.ConvertModelListToStringList(sortedOutsideDataPerTempReverse), true, 85, 2);
            GUI.PrintList("sortedInsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedInsideDataPerHumidityReverse), true, 1, 10);
            GUI.PrintList("sortedOutsideDataPerHumidity", Helpers.ConvertModelListToStringList(sortedOutsideDataPerHumidityReverse), true, 85, 10);

            //Helpers.ConvertModelListToStringList(sortedInsideDataPerTemp);
            //GUI.PrintList("Hej", sortedInsideDataPerTemp, true);

            //List<string> unitList = new List<string>();
            //for (int i = 0; i < insideDataAvgPerDay.Count; i++)
            //{
            //    unitList.Add(insideDataAvgPerDay[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + insideDataAvgPerDay[i].Location.PadRight(13) + "Temperature: " + insideDataAvgPerDay[i].Temp.PadRight(10) + "Humidity: " + insideDataAvgPerDay[i].Humidity + " %");
            //    unitList.Add(outsideDataAvgPerDay[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + outsideDataAvgPerDay[i].Location.PadRight(13) + "Temperature: " + outsideDataAvgPerDay[i].Temp.PadRight(10) + "Humidity: " + outsideDataAvgPerDay[i].Humidity + " %");
            //}

            //GUI.PrintList("Hej", unitList, true);


            //Menu.SelectMonth();

        }
    }
}