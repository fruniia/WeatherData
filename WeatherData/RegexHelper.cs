using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Models;

namespace WeatherData
{
    internal class RegexHelper
    {

        //string regexTest = "^(?<Date>\\b2016\\b-[monthInput]{2}-[dayInput]{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";
        //string regexPattern =@"^(?<Date>\b2016\b-((0)[0-9]|(1)[0-2])-([0-2][0-9]|(3)[0-1]))\s.{8},\b(?<Location>Inne|Ute)\b,(?<Temp>-?\d{1,2}.\d),(?<Humidity>\d{2})$"; //RÖR EJ
        internal static string GetPatternForWholeList()
        {
            return @"^(?<Date>\b2016\b-((0)[0-9]|(1)[0-2])-([0-2][0-9]|(3)[0-1]))\s.{8},\b(?<Location>Inne|Ute)\b,(?<Temp>-?\d{1,2}.\d),(?<Humidity>\d{2})$";
        }
    }
}
