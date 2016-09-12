using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    static class LanguageManipulator
    {
        // if needed, localization goes in this class

        public static List<string> GetHowToDrawMenu()
        {
            List<string> vec = new List<string>();
            
            string[] names = Enum.GetNames(typeof(MenuView.MenuOptions));

            foreach (string str in names)
            {
                StringBuilder strBuild = new StringBuilder(str.ToUpper());

                for (int i = 0; i < strBuild.Length; ++i)
                { 
                    if (!char.IsWhiteSpace(strBuild[i]))
                    {
                        strBuild.Insert(i + 1, " ");
                    }
                }

                vec.Add(strBuild.ToString());
            }

            return vec;
        }
        
        public static string GetHowToDrawBackButton()
        {
            return "B A C K";
        }

        // methods for ResultView

        public static List<string> GetHowToDrawNewResult()
        {
            List<string> vec = new List<string>();

            vec.Add("You achived the highscore");
            vec.Add("Please enter you name: ");

            return vec;
        }

        public static List<string> GetHowToDrawStandart()
        {
            List<string> vec = new List<string>();

            vec.Add("You haven't achived the highscore");
            
            return vec;
        }

        // methods for hints

        public static List<string> GetHowToDrawShipPlacementHint()
        {
            List<string> vec = new List<string>();
            
            vec.Add("Use arrows to move ship and enter to place it.");
            vec.Add("Use q button to rotate ship.");

            return vec;
        }

        public static List<string> GetHowToDrawWinHint()
        {
            List<string> vec = new List<string>();

            vec.Add("You win this game.");
            vec.Add("Press any key to continue..");

            return vec;
        }

        public static List<string> GetHowToDrawLoseHint()
        {
            List<string> vec = new List<string>();

            vec.Add("You lose this game.");
            vec.Add("Press any key to continue..");

            return vec;
        }

        // highscores methods

        public static string GetHighscoresHeader()
        {
            return "H I G H S C O R E S";
        }

        public static List<string> GetHowToDrawDifficulties()
        {
            List<string> vec = new List<string>();

            vec.Add("easy");
            vec.Add("hard");

            return vec;
        }

        // options methods

        public static string GetOptionsHeader()
        {
            return "O P T I O N S";
        }

        public static string GetDifficultyOption(out List<string> vec)
        {
            vec = new List<string>();
            
            vec.Add("e a s y");
            vec.Add("h a r d");

            return "choose difficulty";
        }

        public static string GetColorOption(out List<string> vec)
        {
            vec = new List<string>();
            
            vec.Add("r e d");
            vec.Add("p u r p l e");
            vec.Add("a q u a");

            return "choose color";
        }
    }
}
