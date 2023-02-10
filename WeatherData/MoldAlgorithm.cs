using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void CalculateRiskForMoldGrowth()
        {
            List<SensorDataTime> sensorData = SensorDataTime.GetSensorData();
            double riskForMoldGrowth = 0;
            int maxValue = 150;
            int minValue = 76;
            double asg = (maxValue - minValue) / 100.00;

            foreach(var sensorDataTime in sensorData) 
            {
                double temp = Double.Parse(sensorDataTime.Temp, System.Globalization.CultureInfo.InvariantCulture);
                double humidity = Double.Parse(sensorDataTime.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                    riskForMoldGrowth = (temp + humidity) * asg;
                //if ((temp > 0 || temp < 50) && humidity >= 75)
                //{

                //}
                //else if (temp < 0 || temp > 50 || humidity < 75) //Nollställ risken
                //{
                //    riskForMoldGrowth = 0;
                //}
                Console.WriteLine(riskForMoldGrowth.ToString("0") + " %" + sensorDataTime.Date.ToString());
            }
        }
    }
}

