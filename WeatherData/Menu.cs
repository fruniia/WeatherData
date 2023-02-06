using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class Menu
    {
        internal static int MenuList(string header, int index, List<string> options)
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
            } while (keyPressed.Key != ConsoleKey.Enter);
            Console.CursorVisible = true;
            return index;
        }
    }
}
