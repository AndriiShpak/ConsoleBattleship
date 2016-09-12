using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    class ResultView: BaseView
    {
        int result;

        static readonly int whereTextStarts = 20;
        static readonly int numberOfCharsAllowed = 12;

        public ResultView(int newResult)
        {
            result = newResult;
        }

        public override void Draw()
        {
            string filePath = GetFilePath();

            CleanUnderLogo();

            List<Pair<string, int>> scores = FilesManipulator.GetScores(filePath);

            int indexInVec = NewHighscoreHandler(scores);

            if (indexInVec == -1)
            {
                DrawNotNewHighscoresCase();
            }
            else
            {
                DrawNewHighscoresCase();

                string newName = GetName();

                scores[indexInVec] = new Pair<string, int> {First = newName, Second = result };
                
                FilesManipulator.WriteScores(filePath, scores);
            }

            DrawBackButton();
        }

        string GetFilePath()
        {
            string path = "Resource files/Scores";

            switch(GameLogic.Difficulty)
            {
                case GameLogic.GameDifficulty.Easy:
                    path += "Easy";
                    break;

                case GameLogic.GameDifficulty.Hard:
                    path += "Hard";
                    break;
            }

            
            return path += ".txt";
        }

        int NewHighscoreHandler(List<Pair<string, int>> scores)
        {
            for (int i = 0; i < scores.Count; ++i)
            {
                if (result >= scores[i].Second)
                {
                    return i; // return index in vector in order to save name later
                }
            }

            return -1; // if highscore didn't achieved
        }

        void DrawNotNewHighscoresCase()
        {
            Console.SetCursorPosition(0, whereTextStarts);

            List<string> vec = LanguageManipulator.GetHowToDrawStandart();

            ConsoleHelper.PrintCentered(vec[0], ConsoleColor.DarkRed);
        }

        void DrawNewHighscoresCase()
        {
            Console.SetCursorPosition(0, whereTextStarts);

            List<string> vec = LanguageManipulator.GetHowToDrawNewResult();

            ConsoleHelper.PrintCentered(vec[0], ConsoleColor.DarkCyan);

            int padding = ConsoleHelper.GetPaddingForCenteredText(vec[1]);

            Console.SetCursorPosition(padding, whereTextStarts + 4);

            Console.Write(vec[1]);
        }

        string GetName()
        {
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo info;
            int count = 0;

            while ((info = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    Console.Write(' ');
                    --Console.CursorLeft;
                    --count;
                    sb.Remove(sb.Length - 1, 1);

                    continue;
                }
                
                if (count + 1 > numberOfCharsAllowed) // restricting number of chars in input
                {
                    --Console.CursorLeft;
                    Console.Write(' ');
                    --Console.CursorLeft;

                    continue;
                }
                else
                {
                    ++count;
                }
                    
                sb.Append(info.KeyChar);
            }

            // flushing buffer
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            return sb.ToString();
        }
        
        public override IView Handle()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {/*empty block*/}

            return new MenuView();
        }
    }
}
