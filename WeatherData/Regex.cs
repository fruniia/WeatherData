using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Regex
    {
        
        //string regexTest = "^(?<Date>\\b2016\\b-[monthInput]{2}-[dayInput]{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";
            //string regexPattern = "^(?<Date>\\b2016\\b-\\d{2}-\\d{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$"; //RÖR EJ

        Regex regex = new Regex();
        internal static string GetPattern(string month, string day)
        {
            return "^(?<Date>\\b2016\\b-" + $"[{month}]" + "{2}-" + $"[{day}]" + "{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";

        }

    }
}
