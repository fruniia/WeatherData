using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //List<string> mainMenuText = new()
            //{
            //    "Login",
            //    "Create User",
            //    "Exit"
            //};
            //mainMenuText = ReadData.GetSensorData();

            //Menu.MenuList("Test", 1, mainMenuText, false);

            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();
            Menu.SelectDay(sensorData);
            //Menu.SelectMonth();



        }
    }
}