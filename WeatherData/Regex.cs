using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Regex
    {
        string regex = "^(?<Date>\\b2016\\b-\\d{2}-\\d{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$"; //RÖR EJ
        
        string regexTest = "^(?<Date>\\b2016\\b-[monthInput]{2}-[dayInput]{2})\\s.{8},\\b(?<Location>Inne|Ute)\\b,(?<Temp>-?\\d{1,2}.\\d),(?<Humidity>\\d{2})$";
        public enum RegexMatch
        { 
            Inside,
            Outside,
            Date_day,
            Date_month,
            Date_year
        }

        public static string GetSelectedMatch(string selectedMatch)
        {
            return GetRegexMatch((int)((RegexMatch)Enum.Parse(typeof(RegexMatch), selectedMatch)));
        }
        private static string GetRegexMatch(int selectedIndex)
        {
            string match = "";
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            return match;
        }
    }
}
