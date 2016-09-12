using System;
using System.Collections.Generic;


namespace Battleship
{
    class OptionsView : BaseView
    {
        static readonly int whereToStartDraw = 16;

        // starting selected options
        Selected selectedOption = Selected.Difficulty;
        Colors selectedColor = Colors.Aqua;
        
        // enums for showing which is selected
        enum Selected
        {
            Difficulty,
            Color,
            BackButton
        }
        
        enum Colors
        {
            Red,
            Purple,
            Aqua
        }

        // delegate
        delegate string Option(out List<string> toDraw);


        public override void Draw()
        {
            CleanUnderLogo();

            // drawing header
            Console.SetCursorPosition(0, whereToStartDraw);

            ConsoleHelper.PrintCentered(LanguageManipulator.GetOptionsHeader());

            // drawing options
            DrawOneOption(LanguageManipulator.GetDifficultyOption, 23, selectedOption == Selected.Difficulty, (int)GameLogic.Difficulty);

            DrawOneOption(LanguageManipulator.GetColorOption, 32, selectedOption == Selected.Color, (int)selectedColor);

            // drawing back button based on whether selected or not
            if (selectedOption == Selected.BackButton)
                DrawBackButton();
            else
                DrawBackButton(Console.ForegroundColor); // default color
        }

        void DrawOneOption(Option option, int where, bool isSelectedHeader, int selectedOption)
        {
            string seperator = "    \\     ";
            List<string> toDraw;

            // drawing header of option
            string header = option(out toDraw);

            Console.SetCursorPosition(0, where);

            if (isSelectedHeader)
                ConsoleHelper.PrintCentered(header, basicColor);
            else
                ConsoleHelper.PrintCentered(header);
            
            // getting length of raw of options
            int lengthOfRaw = 0;

            for (int i = 0; i < toDraw.Count; ++i)
            {
                lengthOfRaw += toDraw[i].Length;
            }
            
            lengthOfRaw += (toDraw.Count - 1) * seperator.Length;

            // centering 
            Console.SetCursorPosition(ConsoleHelper.GetPaddingForCenteredText(lengthOfRaw), where + 2);

            // drawing
            for (int i = 0; i < toDraw.Count; ++i)
            {
                // drawing options
                if (i == selectedOption)
                {
                    ConsoleHelper.Write(toDraw[i], basicColor);
                }
                else
                {
                    Console.Write(toDraw[i]);
                }

                // drawing seperator when needed
                if (i != toDraw.Count - 1)
                {
                    Console.Write(seperator);
                }
            }
        }

        public override IView Handle()
        {
            while (true)
            {
                ConsoleKey clickedKey;
                Selected oldSelectedOption = selectedOption;
                
                switch (clickedKey = Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow: //key up
                        if (selectedOption == Selected.Difficulty)
                            break;

                        selectedOption = selectedOption - 1;
                        break;

                    case ConsoleKey.DownArrow: //key down
                        if (selectedOption == Selected.BackButton)
                            break;

                        selectedOption = selectedOption + 1;
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.LeftArrow:
                        if (selectedOption == Selected.BackButton)
                            break;

                        if (HandleRightLeftMove(clickedKey))
                            Draw();

                        break;

                    case ConsoleKey.Enter: // key enter
                        if (selectedOption == Selected.BackButton)
                        {
                            return new MenuView();
                        }

                        break;
                }

                if (oldSelectedOption != selectedOption)
                {
                    Draw();
                }
            }
        }


        bool HandleRightLeftMove(ConsoleKey clickedKey)
        {
            switch (selectedOption)
            {
                case Selected.Difficulty:
                    if (clickedKey == ConsoleKey.LeftArrow && GameLogic.Difficulty == GameLogic.GameDifficulty.Easy ||
                        clickedKey == ConsoleKey.RightArrow && GameLogic.Difficulty == GameLogic.GameDifficulty.Hard)
                    {
                        // if can't make move to the left or right
                        return false;
                    } 
                    
                    if (clickedKey == ConsoleKey.LeftArrow)
                        GameLogic.Difficulty -= 1;
                    else
                        GameLogic.Difficulty += 1;

                    break;

                case Selected.Color:
                    if (clickedKey == ConsoleKey.LeftArrow && selectedColor == Colors.Red ||
                        clickedKey == ConsoleKey.RightArrow && selectedColor == Colors.Aqua)
                    {
                        // if can't make move to the left or right
                        return false;
                    }

                    if (clickedKey == ConsoleKey.LeftArrow)
                        selectedColor -= 1;
                    else
                        selectedColor += 1;

                    HandleColorChange();

                    break;
            }

            return true;
        }

        void HandleColorChange()
        {
            switch(selectedColor)
            {
                case Colors.Red:
                    basicColor = ConsoleColor.DarkRed;
                    break;
                    
                case Colors.Purple:
                    basicColor = ConsoleColor.DarkMagenta;
                    break;

                case Colors.Aqua:
                    basicColor = ConsoleColor.DarkCyan;
                    break;
            }

            DrawLogo();
        }
    }
}
