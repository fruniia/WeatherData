using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Menu
    {
        internal static int MenuList(string header, int index, List<string> options, bool dayMenu)
        {
            ConsoleKeyInfo keyPressed;
            Console.CursorVisible = false;
            do
            {
                GUI.PrintMenu(header, 2, 1, index, options);
                keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.DownArrow && index != options.Count - 1) index++;
                else if (keyPressed.Key == ConsoleKey.UpArrow && index >= 1) index--;
                else if (keyPressed.Key == ConsoleKey.Escape) return -1;
                if (dayMenu)
                {
                    if (keyPressed.Key == ConsoleKey.RightArrow) return -2;
                    else if (keyPressed.Key == ConsoleKey.LeftArrow) return -3;
                }
            } while (keyPressed.Key != ConsoleKey.Enter);
            Console.CursorVisible = true;
            return index;

        }
        internal static void SelectDate()
        {
            int month = 6;
            List<DateTime> dayList = Helpers.GetDates(month);
            while (true)
            {
                int dayIndex = Menu.MenuList("Pick a Date", 0, Helpers.FormatDates(dayList), true);
                if (dayIndex == -2 && month < 12) month++;
                else if (dayIndex == -3 && month > 6 && month > 1) month--;
                else if (dayIndex == -1) return;
                dayList = Helpers.GetDates(month);
            }
        }
    }
}
