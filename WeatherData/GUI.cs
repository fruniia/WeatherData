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
            Console.SetCursorPosition(positionX, positionY -1);
            Console.WriteLine(header);
            for (int y = 0; y < options.Count; y++)
            {
                Console.SetCursorPosition(positionX, positionY + y);
                if (y == index)
                    Console.ForegroundColor= ConsoleColor.Green;

                Console.WriteLine(options[y]);
                Console.ResetColor();
            }
        }
        
        internal static void PrintText(string output)
        {
            //Console.Clear();
            Console.WriteLine(output);
            Console.WriteLine();
        }

        internal static void PrintList (string header, List<string> list, bool printNumbers, int positionX, int positionY)
        {
            Console.SetCursorPosition(positionX, positionY - 1);
            Console.WriteLine(header);
            for (int i = 0; i < list.Count; i++)
            {
                Console.SetCursorPosition(positionX, positionY + i);
                if (printNumbers) Console.Write((i + 1).ToString().PadRight(4));
                Console.Write(list[i]);
            }
        }
    }
}
