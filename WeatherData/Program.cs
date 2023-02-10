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

            List<string> unitList = new List<string>();
            for (int i = 0; i < insideDataAvgPerDay.Count; i++)
            {
                unitList.Add(insideDataAvgPerDay[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + insideDataAvgPerDay[i].Location.PadRight(13) + "Temperature: " + insideDataAvgPerDay[i].Temp.PadRight(10) + "Humidity: " + insideDataAvgPerDay[i].Humidity + " %");
                unitList.Add(outsideDataAvgPerDay[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + outsideDataAvgPerDay[i].Location.PadRight(13) + "Temperature: " + outsideDataAvgPerDay[i].Temp.PadRight(10) + "Humidity: " + outsideDataAvgPerDay[i].Humidity + " %");
            }

            foreach (string data in unitList)
            {
                Console.WriteLine(data);
            }


            //Menu.SelectMonth();


        }
    }
}