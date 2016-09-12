using System;

namespace Battleship
{
    class ConsoleHelper
    {
        static public void PrintCentered(string str)
        {
            int padding = GetPaddingForCenteredText(str);

            Console.CursorLeft = padding;

            Console.WriteLine(str);
        }

        static public void PrintCentered(string str, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            
            PrintCentered(str);

            Console.ResetColor();
        }

        static public int GetPaddingForCenteredText(string str)
        {
            return GetPaddingForCenteredText(str.Length);
        }

        static public int GetPaddingForCenteredText(int length)
        {
            return (Console.WindowWidth - length) / 2 - 1;
        }

        static public void Write(string str, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;

            Console.Write(str);

            Console.ResetColor();
        }

        static public void WriteLine(string str, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            
            Console.WriteLine(str);

            Console.ResetColor();
        }
    }
}
