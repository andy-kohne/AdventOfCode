using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day_13
{
    class Program
    {
        private const int FavoriteNumber = 1362;
        private static char Spot(int x, int y) => Convert.ToString((x * x) + (3 * x) + (2 * x * y) + y + (y * y) + FavoriteNumber, 2).Count(c => c == '1') % 2 == 0 ? ' ' : 'W';

        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            sb.Append("   ");
            for (var x = 0; x < 45; x++)
                sb.Append(x.ToString("00").Substring(1));
            sb.AppendLine();

            for (var y = 0; y < 45; y++)
            {
                sb.AppendFormat("{0:00} ", y);
                for (var x = 0; x < 45; x++)
                {
                    sb.Append(Spot(x, y));
                }
                sb.AppendLine();
            }
            Console.Write(sb);

            Console.ReadKey(true);

            Console.SetCursorPosition(4,2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("S");
            Console.ReadKey(true);
            Console.SetCursorPosition(34, 40);
            Console.Write("E");
            var k = Console.ReadKey(true);

            do
            {

                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.UpArrow)
                    Console.CursorTop--;
                else if (k.Key == ConsoleKey.RightArrow)
                    Console.CursorLeft++;
                else if (k.Key == ConsoleKey.LeftArrow)
                    Console.CursorLeft--;
                else if (k.Key == ConsoleKey.DownArrow)
                    Console.CursorTop++;
                else if (k.KeyChar >= '0' && k.KeyChar <= '9')
                {
                    Console.Write(k.KeyChar);
                    Console.CursorLeft--;
                }
                else if (k.Key == ConsoleKey.Spacebar)
                {
                    Console.Write("X");
                    Console.CursorLeft--;
                }
                else if (k.KeyChar == 'w')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("W");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.CursorLeft--;
                }

            } while (k.Key != ConsoleKey.Escape);

        }

    }
}
