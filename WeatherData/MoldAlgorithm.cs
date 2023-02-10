using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeatherData.Models;

namespace WeatherData
{
    internal class MoldAlgorithm
    {
        public static List<SensorDataTime> CalculateRiskForMoldGrowth(List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> moldRisk = new();
            const double _moldHum = 75;
            const double _scale = 4;
            // ((Temperatur * 2) * ((Humidity - 75) * 4))

            foreach (var data in sensorData)
            {
                double tempPrecent = 0;
                if (Double.Parse(data.Temp) < 50 && Double.Parse(data.Temp) > 0 && Double.Parse(data.Humidity) >= _moldHum)
                    tempPrecent = (2 * Double.Parse(data.Temp));

                double humPrecent = 0;
                if (Double.Parse(data.Humidity) >= _moldHum && tempPrecent > 0.1)
                    humPrecent = ((Double.Parse(data.Humidity) - _moldHum) * _scale);

                double riskForMoldGrowth = Math.Abs((tempPrecent + humPrecent) * 0.5);

                moldRisk.Add(new SensorDataTime
                {
                    Date = data.Date,
                    Temp = data.Temp,
                    Humidity = data.Humidity,
                    Location = data.Location,
                    MoldRisk = riskForMoldGrowth.ToString("0")
                });
            }
            return moldRisk;
        }
    }
}

