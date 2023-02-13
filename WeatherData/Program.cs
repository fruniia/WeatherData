using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();
            List<SensorDataTime> insideDataAvgPerDay = Helpers.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Inside"));
            List<SensorDataTime> outsideDataAvgPerDay = Helpers.GetAvgUnitPerDayList(Helpers.DivideDataPerLocation(sensorData, "Outside"));
            List<SensorDataTime> insideDataAvgPerMonth = Helpers.GetAvgUnitPerMonthList(Helpers.DivideDataPerLocation(sensorData, "Inside"));
            List<SensorDataTime> outsideDataAvgPerMonth = Helpers.GetAvgUnitPerMonthList(Helpers.DivideDataPerLocation(sensorData, "Outside"));

            Menu.MainMenu(sensorData, insideDataAvgPerDay, outsideDataAvgPerDay, insideDataAvgPerMonth, outsideDataAvgPerMonth);
        }
    }
}