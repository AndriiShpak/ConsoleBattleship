using System;
using System.Collections.Generic;

namespace Battleship
{
    class GameLogic
    {
        int score = 0;
        static GameDifficulty gameDifficulty = GameDifficulty.Hard;

        public enum GameDifficulty
        {
            Easy,
            Hard
        }

        public int Score { get { return score; } }
        public static GameDifficulty Difficulty { get { return gameDifficulty; } set { gameDifficulty = value; } }

        public bool HandlePlayerChoice(GameField oponentField, int index_i, int index_j, ref bool isWin)
        {
            if (oponentField.GetIfOnCell(index_i, index_j, GameField.CellType.Ship))
            {
                score += 20;

                int sunkShipSize = oponentField.PlaceHit(index_i, index_j);

                if (sunkShipSize > 0)
                {
                    score += 200 - sunkShipSize * 40;
                }

                if (oponentField.NumerOfShipsLeft == 0)
                {
                    score += 400;

                    isWin = true;
                    
                    return false; // if win
                }
                
                return false; // if hit
            }
            else if (oponentField.GetIfOnCell(index_i, index_j, GameField.CellType.Empty))
            {
                oponentField.PlaceMiss(index_i, index_j);

                return true; // if miss
            }
            
            return false; // if bad input choic (already hit or miss on cell)
        }

        public bool MakeOponentChoice(GameField playerField, ref bool isLose)
        {
            Pair<int, int> toCheck = new Pair<int, int> {First = -1, Second = -1};
            
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    // just random choice
                    toCheck = EasyOponentChoice(playerField);
                    
                    break;

                case GameDifficulty.Hard:
                    // random choice but priority on shooting around hit ship 
                    toCheck = HardOponentChoice(playerField);
                    
                    break;
            }


            if (playerField.GetIfOnCell(toCheck.First, toCheck.Second, GameField.CellType.Ship))
            {
                playerField.PlaceHit(toCheck.First, toCheck.Second);
                
                if (playerField.NumerOfShipsLeft == 0)
                {
                    isLose = true;
                    return false; // don't need to make new move
                }

                return true; // need to make move again
            }
            else if ((playerField.GetIfOnCell(toCheck.First, toCheck.Second, GameField.CellType.Empty)))
            {
                playerField.PlaceMiss(toCheck.First, toCheck.Second);
            }

            return false; // oponent choice next
        }

        Pair<int, int> EasyOponentChoice(GameField playerField)
        {
            // just random choice

            Random randomGenerator = new Random();

            List<Pair<int, int>> vecOfPossible = new List<Pair<int, int>>();

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (!playerField.GetIfOnCell(i, j, GameField.CellType.Miss) &&
                                                 !playerField.GetIfOnCell(i, j, GameField.CellType.HitShip))
                    {
                        vecOfPossible.Add(new Pair<int, int> { First = i, Second = j});
                    }
                }
            }

            return vecOfPossible[randomGenerator.Next(vecOfPossible.Count)]; 
        }

        Pair<int, int> HardOponentChoice(GameField playerField)
        {
            // random choice but priority on shooting around hit ship 

            Random randomGenerator = new Random();

            Pair<int, int> toCheck = new Pair<int, int>{ First = -1, Second = -1 };
            List<Pair<int, int>> vecOfPossible = new List<Pair<int, int>>();

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (playerField.GetIfOnCell(i, j, GameField.CellType.HitShip))
                    {
                        // here some sweet logic

                        searchForPossibleAround(playerField, ref toCheck, i, j); //recursive function

                        if (toCheck.First != -1) // if some variant around hit found 
                        {
                            break;
                        }
                    }
                    else if (!playerField.GetIfOnCell(i, j, GameField.CellType.Miss))
                    {
                        // just add to possible variants

                        vecOfPossible.Add(new Pair<int, int> { First = i, Second = j });
                    }
                }

                if (toCheck.First != -1) // if some variant around hit found
                {
                    break;
                }
            }

            if ((toCheck.First == -1))
            {
                // if nothing nice found than just random

                toCheck = vecOfPossible[randomGenerator.Next(vecOfPossible.Count)];
            }

            return toCheck;
        }

        bool searchForPossibleAround(GameField playerField, ref Pair<int, int> toCheck, 
                                        int index_i, int index_j, int from_i = -1, int from_j = -1)
        {
            // recursive function for hard level logic

            
            List<Pair<int, int>> somePossible = new List<Pair<int, int>>();

            for (int i = -1; i <= 1; ++i)
            {
                if (index_i + i < 0 || index_i + i >= 10) //check for edge of field
                    continue;

                for (int j = -1; j <= 1; ++j)
                {

                    if (index_j + j < 0 || index_j + j >= 10) // check for edge of field
                        continue;

                    if (index_i + i == from_i && index_j + j == from_j) // in order not to cause infinite recursive call
                        continue;

                    if ((j == -1 && i == -1) || (j == -1 && i == 1) || (j == 1 && i == -1) || (j == 1 && i == 1) || (j == 0 && i == 0))
                    {
                        // exclude pairs (-1, -1), (-1, 1), (1, -1), (1, 1) and (0, 0)
                        continue;
                    }

                    // check for hit ship
                    if (playerField.GetIfOnCell(index_i + i, index_j + j,GameField.CellType.HitShip))
                    {
                        if (searchForPossibleAround(playerField, ref toCheck, index_i + i, index_j + j, index_i, index_j)) // recursive call
                            return true;
                    }
                    else if (!playerField.GetIfOnCell(index_i + i, index_j + j, GameField.CellType.Miss))
                    {
                        // if found possible cell around hit ship
                        somePossible.Add(new Pair<int, int> { First = index_i + i, Second = index_j + j });
                    }
                }
            }

            // now from possible around hit ship choose one
            if (somePossible.Count != 0)
            {
                Random random = new Random();

                toCheck = somePossible[random.Next(somePossible.Count)];

                return true; // don't need to look further
            }

            return false; // keep looking for possible cell
        }
    }
}
