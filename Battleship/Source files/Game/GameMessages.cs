using System;
using System.Collections.Generic;

namespace Battleship
{
    static class GameMessages
    {
        // frame-box of messages
        static readonly int frameWidth = 45;
        static readonly int frameHeight = 8;

        static readonly BaseView.Point whereFrameStarts = new BaseView.Point { X = Console.WindowWidth / 2 - frameWidth / 2 - 2, Y = 40 };
        
        // delegate
        public delegate List<string> Hint();

        // methods
        public static void ClearHints()
        {
            string space = new string(' ', Console.WindowWidth - 2);

            for (int i = 0; i < frameHeight; ++i)
            {
                Console.SetCursorPosition(0, whereFrameStarts.Y + i);

                Console.Write(space);
            }
        }

        public static void DrawHint(Hint getHintToDraw)
        {
            List<string> toDraw = getHintToDraw();

            for (int i = 0; i < toDraw.Count; ++i)
            {
                Console.SetCursorPosition(whereFrameStarts.X, whereFrameStarts.Y + 2 + 2 * i);
                
                ConsoleHelper.PrintCentered(toDraw[i], ConsoleColor.DarkGray);
            }

        }
        
    }
}