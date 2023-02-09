using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class RegexHelper
    {

        //string regexTest = "^(?<Date>\\b2016\\b-[monthInput]{2}-[dayInput]{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";
        //string regexPattern =@"^(?<Date>\b2016\b-((0)[0-9]|(1)[1-2])-([0-2][0-9]|(3)[0-1]))\s.{8},\b(?<Location>Inne|Ute)\b,(?<Temp>-?\d{1,2}.\d),(?<Humidity>\d{2})$"; //RÖR EJ
        internal static string GetPatternForWholeList()
        {
            return @"^(?<Date>\b2016\b-((0)[0-9]|(1)[1-2])-([0-2][0-9]|(3)[0-1]))\s.{8},\b(?<Location>Inne|Ute)\b,(?<Temp>-?\d{1,2}.\d),(?<Humidity>\d{2})$";
        }
        internal static string GetPattern(string month, string day)
        {
            return " ^ (?<Date>\\b2016\\b-" + $"[{month}]" + "{2}-" + $"[{day}]" + "{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";

        }
        internal static List<string> GetMatchValue(string pattern, List<string> sensorData)
        {
            List<string> result = new List<string>();
            Regex regex = new Regex(pattern);

            foreach (var data in sensorData)
            {
                MatchCollection matches = regex.Matches(data);
                if (matches.Count != 0)
                {
                    result.Add(matches[0].ToString());
                }
            }
            return result;
        }

    }
}
