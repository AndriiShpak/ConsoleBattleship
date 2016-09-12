using System;
using System.Collections.Generic;

namespace Battleship
{
    class HighscoresView: BaseView
    {
        static readonly int whereToStartDraw = 16;

        public override void Draw()
        {
            CleanUnderLogo();

            Console.SetCursorPosition(0, whereToStartDraw );

            ConsoleHelper.PrintCentered(LanguageManipulator.GetHighscoresHeader());

            DrawLines();

            DrawDifficulties();

            DrawScores("Resource files/ScoresEasy.txt", new Point { X = 12, Y = whereToStartDraw + 10 });

            DrawScores("Resource files/ScoresHard.txt", new Point { X = 42, Y = whereToStartDraw + 10 });
            
            DrawBackButton();            
        }

        void DrawLines()
        {
            Point current = new Point{ X = 0, Y = whereToStartDraw + 5 };

            Console.SetCursorPosition(current.X, current.Y);
            
            ConsoleHelper.PrintCentered(new string('.', 58)); // horizontal line
            
            for (int i = 0; i < 20; ++i) // vertical line
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 2, current.Y + i);

                Console.Write('.');
            }
        }

        void DrawDifficulties()
        {
            List<string> diff = LanguageManipulator.GetHowToDrawDifficulties();

            for (int i = 0; i < 2; ++i)
            {
                int whereX;

                // which part of screen
                if (i == 0)
                    whereX = Console.WindowWidth / 4 + 3;
                else
                    whereX = Console.WindowWidth * 3 / 4 - 5;

                // centering string in part of screen
                whereX -= diff[i].Length / 2;

                // printing
                Console.SetCursorPosition(whereX, whereToStartDraw + 7);

                ConsoleHelper.Write(diff[i], basicColor);
            }
        }

        void DrawScores(string filePath, Point where)
        {
            int scoresWidth = 23;

            List<Pair<string, int>> scores = FilesManipulator.GetScores(filePath);
            
            for (int i = 0; i < scores.Count; ++i)
            {
                Console.SetCursorPosition(where.X, where.Y + i * 2);

                Console.Write(scores[i].First);

                string scoreToDraw = scores[i].Second.ToString();

                Console.SetCursorPosition(where.X + scoresWidth - scoreToDraw.Length, where.Y + i * 2);

                Console.Write(scoreToDraw);
            }
            
        }

        public override IView Handle()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {/*empty block*/}

            return new MenuView();
        }
    }
}
