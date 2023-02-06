using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData
{
    internal class GUI
    {
        internal static void PrintMenu(string header, int positionX, int positionY, int index, List<string> options)
        {
            for (int y = 0; y < options.Count; y++)
            {
                Console.SetCursorPosition(positionX, positionY + y);
                if (y == index)
                    Console.ForegroundColor= ConsoleColor.Green;

                Console.WriteLine(options[y]);
                Console.ResetColor();
            }
        }
    }
}
