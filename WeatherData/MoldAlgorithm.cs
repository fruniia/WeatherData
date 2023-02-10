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
        

        /*◦ I uppgiften ingår att skapa en
            algoritm/formel för mögelrisk.
            ◦ Diskutera I Gruppen
            ◦ Försök förenkla detta till en hanterbar
            formel, som fungerar för alla data I filen.
            ◦ Skapa en skala för risk mellan 0-100
         */

        // MAX 150 -> MIN 76

        //Skillnad är 74 -> skala 1 - 100 (74/100)
        //Hämta temperatur
        //Hämta fukt
        //Hämta location

        //Vilken temperatur är det risk för mögel >0 && < 50
        //Fuktprocent över 75 % kan det börja växa

        public static List<SensorDataTime> CalculateRiskForMoldGrowth(List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> moldRisk = new();
            double riskForMoldGrowth = 0;
            const double asg = (150 - 76) / 100.00; //Maxvalue 150 Minvalue 76
            double temp = 0;
            double humidity = 0;

            foreach (var sensorDataTime in sensorData)
            {
              
                temp += Double.Parse(sensorDataTime.Temp);
                humidity += Double.Parse(sensorDataTime.Humidity);
                riskForMoldGrowth = ((temp + humidity) * asg);
                temp = 0;
                humidity = 0;
                moldRisk.Add(  new SensorDataTime
                {
                    Date = sensorDataTime.Date,
                    Temp = sensorDataTime.Temp,
                    Humidity = sensorDataTime.Humidity,
                    Location = sensorDataTime.Location,
                    MoldRisk = riskForMoldGrowth.ToString("0")
                });

            }

            return moldRisk;

        }
        //public static List<SensorDataTime> CalculateAverageRiskForMoldGrowth(List<SensorDataTime> sensorData)
        //{

        //}
    }
}

