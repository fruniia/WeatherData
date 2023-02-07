using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Helpers
    {
        public static string GetStringFromUser(string prompt)
        {
            Console.Write(prompt);
            string? result = Console.ReadLine();
            result = result.Any(x => char.IsLetterOrDigit(x)).ToString();
            if (result == null)
            {
                result = "";
            }
            return result;
        }
        internal static List<DateTime> GetDates(int month, int year = 2016)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .ToList();
        }
        internal static List<string> FormatDates(List<DateTime> dates)
        {
            List<string> formattedDates = new();
            for (int i = 0; i < dates.Count; i++)
            {
                formattedDates.Add(dates[i].DayOfWeek.ToString().PadRight(10) + ": " + dates[i].ToString("yyyy/MM/dd"));
            }
            return formattedDates;
        }

        internal static void CompareSelectedDateWithRegex(DateTime date)
        {
            string pattern = Regex.GetPattern(ReturnFormattedDate(date.Month.ToString()), ReturnFormattedDate(date.Day.ToString()));
            Console.WriteLine(pattern);
            Console.ReadLine();

        }

        internal static string ReturnFormattedDate(string date)
        {
            if (date.Length == 1) return "0" + date;
            return date;
        }
        //internal static List<string> GetSelectedData(string regex)
        //{ 

        //}
    }
}
