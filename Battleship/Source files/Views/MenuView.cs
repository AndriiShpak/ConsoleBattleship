using System;
using System.Collections.Generic;

namespace Battleship
{
    class MenuView : BaseView
    {
        List<string> howToDraw = LanguageManipulator.GetHowToDrawMenu();

        MenuOptions selectedOption = MenuOptions.Start;

        readonly int whereMenuStarts = 20;
        readonly int numberEmptyLinesInMenu = 4;

        public enum MenuOptions
        {
            Start,
            Highscores,
            Options,
            Exit
        };

        public MenuView()
        {
            Clean();
        }

        public override void Draw()
        {
            Clean(whereMenuStarts);

            DrawLogo();

            DrawMenu();
        }

        void DrawMenu()
        {
            for (int i = 0; i < howToDraw.Count; ++i)
            {
                Console.SetCursorPosition(0, whereMenuStarts + i * numberEmptyLinesInMenu);

                string toDraw = howToDraw[i];

                // showing selected option
                if (i == (int)selectedOption)
                {
                    toDraw = toDraw.Insert(toDraw.Length, howToShowSelected[1]).Insert(0, howToShowSelected[0]);

                    ConsoleHelper.PrintCentered(toDraw, basicColor);
                }
                else
                {
                    ConsoleHelper.PrintCentered(toDraw);
                }
                
            }
        }

        public override IView Handle()
        {
            while (true)
            {
                MenuOptions oldSelectedOption = selectedOption;

                // handling moves and changes
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow: //key up
                        if (selectedOption == MenuOptions.Start) break;
                        selectedOption = selectedOption - 1; 
                       break;

                    case ConsoleKey.DownArrow: //key down
                        if (selectedOption == MenuOptions.Exit) break;
                        selectedOption = selectedOption + 1;
                        break;

                    case ConsoleKey.Enter: // key enter
                        return GetNextView();

                    default:
                        continue;
                }

                if (oldSelectedOption != selectedOption)
                {
                    // if something changed than redraw

                    Draw();
                }
            }
        }

        IView GetNextView()
        {
	         switch (selectedOption)
	         {
		         case MenuOptions.Start:
			         return new GameView();

		         case MenuOptions.Options:
			         return new OptionsView();

		         case MenuOptions.Highscores:
			         return new HighscoresView();

		         case MenuOptions.Exit:
			          return null;

                default:
                    throw new Exception("not all return paths is handled in MenuView::GetNextView()");
	         }
        }
    }
}
