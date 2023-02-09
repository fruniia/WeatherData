using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherData.Data;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace WeatherData.Models
{
    internal class SensorDataTime
    {
        public DateTime Date { get; set; }
        public string Temp { get; set; }
        public string Humidity { get; set; }
        public string Location { get; set; }

        internal static List<SensorDataTime> GetSensorData()
        {
            List<string> sensorData = ReadData.GetSensorData();
            Regex pattern = new Regex(RegexHelper.GetPatternForWholeList());
            List<SensorDataTime> sensorDataTime = new List<SensorDataTime>();

            foreach (string data in sensorData)
            {
                Match match = pattern.Match(data);
                try
                {
                    if (match.Success)
                    {

                        sensorDataTime.Add(new SensorDataTime
                        {
                            Date = DateTime.ParseExact(match.Groups["Date"].Value, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                            Temp = match.Groups["Temp"].Value,
                            Humidity = match.Groups["Humidity"].Value,
                            Location = (match.Groups["Location"].Value == "Inne") ? "Inside" : "Outside"

                        }); ;
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    Console.ReadLine();
                }
            }
            return sensorDataTime;
        }
    }
}
