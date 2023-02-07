using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Regex
    {
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
