using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();
            List<SensorDataTime> insideDataAvgPerDay = Menu.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Inside"));
            List<SensorDataTime> outsideDataAvgPerDay = Menu.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Outside"));
            List<SensorDataTime> insideDataAvgPerMonth = Menu.GetAvgUnitPerMonthList(Helpers.DivideDataPerLocation(sensorData, "Inside"));
            List<SensorDataTime> outsideDataAvgPerMonth = Menu.GetAvgUnitPerMonthList(Helpers.DivideDataPerLocation(sensorData, "Outside"));

            Menu.MainMenu(sensorData, insideDataAvgPerDay, outsideDataAvgPerDay, insideDataAvgPerMonth, outsideDataAvgPerMonth);
        }
    }
}