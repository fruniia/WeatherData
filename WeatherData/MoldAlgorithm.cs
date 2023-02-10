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

        public static void CalculateRiskForMoldGrowth(List<SensorDataTime> sensorData)
        {
            double riskForMoldGrowth = 0;
            const double asg = (150 - 76) / 100.00; //Maxvalue 150 Minvalue 76
            int counter = 0;
            double temp = 0;
            double humidity = 0;
            foreach(var sensorDataTime in sensorData) 
            {
                temp += Double.Parse(sensorDataTime.Temp, System.Globalization.CultureInfo.InvariantCulture);
                humidity += Double.Parse(sensorDataTime.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                counter++;
                //if ((temp > 0 || temp < 50) && humidity >= 75)
                //{

                //}
                //else if (temp < 0 || temp > 50 || humidity < 75) //Nollställ risken
                //{
                //    riskForMoldGrowth = 0;
                //}

            }
                riskForMoldGrowth = ((temp + humidity)/counter) * asg;
                Console.WriteLine(riskForMoldGrowth.ToString("0") + " %");
        }
    }
}

