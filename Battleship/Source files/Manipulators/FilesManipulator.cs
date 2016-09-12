using System;
using System.Collections.Generic;

namespace Battleship
{
    static class FilesManipulator
    {
        public static List<string> GetStringsFromFile(string fileName)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);

            List<string> vec = new List<string>();
            string str;

            while((str = file.ReadLine()) != null)
            {
                vec.Add(str);
            }

            return vec;
        }

        public static List<Pair<string, int>> GetScores(string fileName)
        {
            List<Pair<string, int>> scores = new List<Pair<string, int>>();
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
            {
                string name;
                int score;

                while ((name = file.ReadLine()) != null)
                {
                    int.TryParse(file.ReadLine(), out score);

                    scores.Add(new Pair<string, int> { First = name, Second = score });
                }
            }

            return scores;
        }

        public static void WriteScores(string fileName, List<Pair<string, int>> scores)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                foreach (var i in scores)
                {
                    file.WriteLine(i.First);
                    file.WriteLine(i.Second);
                }
            }
        }
    }
}
