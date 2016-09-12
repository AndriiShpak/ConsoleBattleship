using System;

namespace Battleship
{
    class GreetingsView : BaseView
    {
        static readonly int whereGreetingLogoStarts = 19;

        public override void Draw()
        {
            Console.SetCursorPosition(13, whereGreetingLogoStarts - 1);

            Console.Write("Welcome to...");

            DrawLogo(whereGreetingLogoStarts);

            DrawAuthor();

            WaitBlinking("PRESS ANY KEY TO START", 27); // on 27 row from top
        }
        
        void DrawAuthor()
        {
            string toPrint = "Coded by Andriy Shpak. 2016. ";

            Console.SetCursorPosition(Console.WindowWidth - toPrint.Length, Console.WindowHeight - 1);

            Console.Write(toPrint);
        }

        void WaitBlinking(string toPrint, int whereTo)
        {
            System.Timers.Timer timer = new System.Timers.Timer(700);
            
            bool isHide = false;
            bool isNeedToStartTimer = true;
            
            timer.Elapsed +=
                        (Object source, System.Timers.ElapsedEventArgs ee) => 
                                                    DrawBlinkingPhrase(whereTo, toPrint, ref isHide, ref isNeedToStartTimer);
            
            while (!Console.KeyAvailable)
            {
                if (isNeedToStartTimer)  // needed to make interval
                {
                    // isNeedToStartTimer is assigned true when trigerred DrawBlinkingPhrase  (when timer goes off)

                    timer.Enabled = true;
                    isNeedToStartTimer = false;
                }
            }

            Console.ReadKey(); // clearing button that was clicked
            timer.Enabled = false;
        }

        void DrawBlinkingPhrase(int whereTo, string toPrint, ref bool isHide, ref bool isNeedToStartTimer)
        {
            Console.SetCursorPosition(0, whereTo);

           if (isHide)
           {
               string emptyLine = new string(' ', toPrint.Length);
               ConsoleHelper.PrintCentered(emptyLine);
           }
           else
           {
               ConsoleHelper.PrintCentered(toPrint);
           }

           isHide = isHide ? false : true; // changing to oposite

           isNeedToStartTimer = true; // to start new timer
        }
        
        public override IView Handle()
        {
            return new MenuView();
        }
    }
}
