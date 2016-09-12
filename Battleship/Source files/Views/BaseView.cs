using System;
using System.Collections.Generic;

namespace Battleship
{
    abstract class BaseView : IView
    {
        // where to draw components
        static readonly int whereLogoStarts = 6;
        static readonly int whereBackButton = Console.WindowHeight - 6;

        // how to draw some elements
        static readonly List<string> logo = FilesManipulator.GetStringsFromFile("Resource files/Logo.txt");
        protected readonly string[] howToShowSelected = { "*  ", "  *" };

        // basic color
        protected static ConsoleColor basicColor = ConsoleColor.DarkCyan;

        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
        
        protected void Clean(int whereToStart)
        {
            string replaceText = new string(' ', Console.WindowWidth - 1);

            for (int i = whereToStart; i < Console.WindowHeight; ++i)
            {
                Console.SetCursorPosition(0, i);
                
                Console.Write(replaceText);
            }
        }

        protected void Clean()
        {
            Console.Clear();
        }

        protected void CleanUnderLogo()
        {
            Clean(whereLogoStarts + logo.Count + 1);
        }

        protected void DrawLogo(int whereTo)
        {
            int padding = ConsoleHelper.GetPaddingForCenteredText(logo[0]) + 2;

            for (int i = 0; i < logo.Count; ++i)
            {
                Console.SetCursorPosition(padding, whereTo + i);
                ConsoleHelper.Write(logo[i], basicColor);
            }
        }

        protected void DrawLogo()
        {
            DrawLogo(whereLogoStarts);
        }

        protected void DrawBackButton(ConsoleColor color)
        {
            Console.SetCursorPosition(0, whereBackButton);

            string backButton = LanguageManipulator.GetHowToDrawBackButton();

            backButton = backButton.Insert(backButton.Length, howToShowSelected[1]).Insert(0, howToShowSelected[0]); ;

            ConsoleHelper.PrintCentered(backButton, color);
        }

        protected void DrawBackButton()
        {
            DrawBackButton(basicColor);
        }
        
        // From IView
        public abstract void Draw();
        public abstract IView Handle();
    }
}
