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
            Menu.GetHumidityForAllMonths(sensorData);
            Menu.GetHottestDays(sensorData);
            Menu.GetColdestDays(sensorData);
            //Menu.SelectMonth();


        }
    }
}