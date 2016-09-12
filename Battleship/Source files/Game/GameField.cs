using System;
using System.Collections.Generic;

namespace Battleship
{
    class GameField
    {
        static readonly int gameSize = 10;
        static readonly int cellHorizontalSize = 3;
        static readonly int cellVerticalSize = 2;

        // getting array of how to draw field
        List<string> howToDrawField = FilesManipulator.GetStringsFromFile("Resource files/GameField.txt");

        // values that describe field
        BaseView.Point whereFieldStarts;
        FieldType fieldType;

        // cells array
        CellType[,] field = new CellType[gameSize, gameSize];

        // number of ships
        int numberOfShipsLeft = 4 + 3 + 2 + 1;

        public int NumerOfShipsLeft { get { return numberOfShipsLeft; } }
        


        public enum FieldType
        {
            PlayerField,
            OponentField
        }
        
        public enum Direction
        {
            // enum for ship placement

            Horizontal,
		    Vertical
        };


        enum DrawCellType
        {
            // enum for ship placement

            Empty,
            Ship,
            HitShip,
            Miss,
            BadPut
        };

        public enum CellType
        {
            // enum for cells array

            Empty,
            Ship,
            HitShip,
            Miss,
        }
       

        // constructor
        public GameField(int left, int top, FieldType type)
        {
            whereFieldStarts = new BaseView.Point { X = left, Y = top };
            fieldType = type;
        }
        
        public void DrawField()
        {
            for (int i = 0; i < howToDrawField.Count; ++i) 
            {
                // 2 chars for digits and empty space before field

                Console.SetCursorPosition(whereFieldStarts.X - 2, whereFieldStarts.Y + i - 2); 
                ConsoleHelper.Write(howToDrawField[i], GetColorsForCell(DrawCellType.Empty));

            }
            
            // in order not to crush display, it should drawn in turns
            if (fieldType == FieldType.PlayerField)
            {
                // in oponent field ships shouldn't be displayed
                DrawCellsOfCertainType(CellType.Ship);
            }
           
            DrawCellsOfCertainType(CellType.Miss);
            DrawCellsOfCertainType(CellType.HitShip);           
        }

        void DrawCellsOfCertainType(CellType type)
        {
            // draw cells of one type

            for (int i = 0; i < gameSize; ++i)
                for (int j = 0; j < gameSize; ++j)
                    if (type == field[i, j])
                    {
                        DrawCell(i, j, (DrawCellType)field[i, j]);
                    }
        }

        void DrawCell(int index_i, int index_j, DrawCellType type)
        {
            // drawing one cell

            Console.ForegroundColor = GetColorsForCell(type);

            BaseView.Point cellCoord = new BaseView.Point
            {
                // coord where cell starts

                X = whereFieldStarts.X + index_j * cellHorizontalSize,
                Y = whereFieldStarts.Y + index_i * cellVerticalSize - 1
            };

            if (type == DrawCellType.Miss)
            {
                Console.SetCursorPosition(cellCoord.X + 1, cellCoord.Y + 1);
                Console.Write(",,");
            }
            else
            {
                Console.SetCursorPosition(cellCoord.X + 1, cellCoord.Y);
                
                Console.Write("__");

                for (int k = 0; k < 2; ++k)
                {
                    Console.SetCursorPosition(cellCoord.X, cellCoord.Y + k + 1);

                    if (k == 0)
                         Console.Write("|  |");
                    else
                        Console.Write("|__|");
                }
            }

            Console.ResetColor();
        }

        ConsoleColor GetColorsForCell(DrawCellType type)
        {
            switch (type)
            {
                case DrawCellType.Empty:
                    return ConsoleColor.DarkYellow;
                    
                case DrawCellType.Ship:
                    return ConsoleColor.DarkCyan;
                   
                case DrawCellType.HitShip:
                    return ConsoleColor.DarkRed;
                    
                case DrawCellType.BadPut:
                    return ConsoleColor.DarkRed;
                   
                case DrawCellType.Miss:
                    return ConsoleColor.DarkYellow;
                    
                default:
                    throw new Exception("not all paths handled in GameField::GetColorsForCell()");

            }
        }
        
        public void DrawSelectedCell(int index_i, int index_j)
        {
            // drawing where cursor

            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            if (fieldType == FieldType.OponentField && field[index_i, index_j] == CellType.Ship)
            {
                // in order not to show oponent ships when move 

                Console.ForegroundColor = GetColorsForCell((DrawCellType.Empty));
            }
            else
            {
                Console.ForegroundColor = GetColorsForCell((DrawCellType)field[index_i, index_j]);
            }
           

            BaseView.Point cellCoord = new BaseView.Point
            {
                // coord of cell

                X = whereFieldStarts.X + index_j * cellHorizontalSize,
                Y = whereFieldStarts.Y + index_i * cellVerticalSize - 1
            };
            
            for (int k = 0; k < 2; ++k)
            {
                Console.SetCursorPosition(cellCoord.X + 1, cellCoord.Y + k + 1);

                // changing only background of cell
                if (k == 0)
                    if (field[index_i, index_j] == CellType.Miss)
                        Console.Write(",,");
                    else
                        Console.Write("  ");
                else
                    Console.Write("__");
                  
            }

            Console.ResetColor();
        }

        public void DrawNewShipPosition(int index_i, int index_j, int size, Direction direction) // method for ship placement
        {
            // firstly draw ship
            switch (direction)
            {
                case Direction.Horizontal:
                    for (int k = 0; k < size; ++k)
                    {
                        DrawCell(index_i, index_j + k, DrawCellType.Ship);
                    }

                    break;

                case Direction.Vertical:
                    for (int k = 0; k < size; ++k)
                    {
                        DrawCell(index_i + k, index_j, DrawCellType.Ship);
                    }

                    break;
            }

            // secondly draw when intersect with another ship or can't be placed

            switch (direction)
            {
                case Direction.Horizontal:
                    for (int k = 0; k < size; ++k)
                    {
                        if (!IsOkayToPlaceShip(index_i, index_j + k))
                            DrawCell(index_i, index_j + k, DrawCellType.BadPut);
                    }

                    break;

                case Direction.Vertical:
                    for (int k = 0; k < size; ++k)
                    {
                        if (!IsOkayToPlaceShip(index_i + k, index_j))
                            DrawCell(index_i + k, index_j, DrawCellType.BadPut);
                    }

                    break;
            }

        }

        bool IsOkayToPlaceShip(int index_i, int index_j)
        {
            // checking whether okay to place ship in cell
            
            for (int i = -1; i <= 1; ++i)
            {
                if (index_i + i < 0 || index_i + i >= gameSize)
                    continue;

                for (int j = -1; j <= 1; ++j)
                {
                    if (index_j + j < 0 || index_j + j >= gameSize)
                        continue;
                    
                    if (field[index_i + i, index_j + j] == CellType.Ship)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsOkayToPlaceShip(int index_i, int index_j, int size, Direction direction)
        {
            // checking whether okay to place ship in cells

            switch (direction)
            {
                case Direction.Horizontal:
                    for (int k = 0; k < size; ++k)
                    {
                        if (index_j + k >= gameSize)
                            return false;

                        if (!IsOkayToPlaceShip(index_i, index_j + k))
                            return false;
                    }

                    break;

                case Direction.Vertical:
                    for (int k = 0; k < size; ++k)
                    {
                        if (index_i + k >= gameSize)
                            return false;

                        if (!IsOkayToPlaceShip(index_i + k, index_j))
                            return false;
                    }

                    break;
            }

            return true;
        }

        public void PlaceShip(int index_i, int index_j, int size, Direction direction)
        {
            // assigning values in array of cells (where ship is placed)

            switch (direction)
            {
                case Direction.Horizontal:
                    for (int k = 0; k < size; ++k)
                    {
                        field[index_i, index_j + k] = CellType.Ship;
                    }

                    break;

                case Direction.Vertical:
                    for (int k = 0; k < size; ++k)
                    {
                        field[index_i + k, index_j] = CellType.Ship;
                    }

                    break;
            }
        }

        public void PlaceMiss(int index_i, int index_j)
        {
            field[index_i, index_j] = CellType.Miss;

            DrawField();
        }

        public void RandomShipPlacement()
        {
            Random randomGenerator = new Random();
            
            for (int size = 4; size >= 1; --size) // ship size
            {
                for (int number = 1; number <= 5 - size; ++number) // number of ships
                {
                    Direction direction = (Direction)(randomGenerator.Next(2)); // 0 or 1 by random

                    // filling with all possible positions of ship
                    List<Pair<int, int>> vecOfPossiblePositions = new List<Pair<int, int>>();
                    
                    for (int ii = 0; ii < gameSize; ++ii)
                    {
                        for (int jj = 0; jj < gameSize; ++jj)
                        {
                            if (IsOkayToPlaceShip(ii, jj, size, direction))
                            {
                                vecOfPossiblePositions.Add(new Pair<int, int> { First = ii, Second = jj });
                            }
                        }
                    }

                    // choosing one from possible
                    Pair<int, int> toPlace = vecOfPossiblePositions[ randomGenerator.Next(vecOfPossiblePositions.Count)];

                    // placing selected
                    PlaceShip(toPlace.First, toPlace.Second, size, direction);
                }
            }
        }
        
        public void RenewField()
        {
            for (int i = 0; i < gameSize; ++i)
                for (int j = 0; j < gameSize; ++j)
                {
                    field[i, j] = CellType.Empty;
                }

            numberOfShipsLeft = 4 + 3 + 2 + 1; // number of all ships
        }

        public bool GetIfOnCell(int index_i, int index_j, CellType toCheck)
        {
            // get if cell has specific type

            return field[index_i, index_j] == toCheck;
        }

        public int PlaceHit(int index_i, int index_j)
        {
            // placing hit

            field[index_i, index_j] = CellType.HitShip;

            DrawCell(index_i, index_j, DrawCellType.HitShip);

            // checking whether ship is sunk and defining size if sunk
            int sunkShipSize = 0;

            if (CheckIfWholeShipHit(index_i, index_j, ref sunkShipSize))
            {
                --numberOfShipsLeft;
                DrawAroundSunkShip(index_i, index_j);

                return sunkShipSize;
            }
            

            return -1; // ship is not sunk case
        }

        bool CheckIfWholeShipHit(int index_i, int index_j, ref int sunkShipSize, int fromWhichCalled_i = -1, int fromWhichCalled_j = -1)
        {
            // recursive function

            ++sunkShipSize;

            for (int i = -1; i <= 1; ++i)
            {
                if (index_i + i < 0 || index_i + i >= 10) //check for edge of field
                    continue;

                for (int j = -1; j <= 1; ++j)
                {

                    if (index_j + j < 0 || index_j + j >= 10) // check for edge of field
                        continue;

                    if (index_i + i == fromWhichCalled_i && index_j + j == fromWhichCalled_j) // in order not to cause infinite recursive call
                        continue;

                    if ((j == -1 && i == -1) || (j == -1 && i == 1) || (j == 1 && i == -1) || (j == 1 && i == 1) || (j == 0 && i == 0))
                    {
                        // exclude pairs (-1, -1), (-1, 1), (1, -1), (1, 1) and (0, 0)
                        continue;
                    }

                    // check for non hit ship
                    if (field[index_i + i, index_j + j] == CellType.Ship)
                    {
                        return false; // if cell with ship around than ship is not sunk
                    }
                    else if (field[index_i + i, index_j + j] == CellType.HitShip)
                    {
                        if (!CheckIfWholeShipHit(index_i + i, index_j + j, ref sunkShipSize, index_i, index_j)) // recursive call of function
                            return false; 
                    }
                }
            }

            return true; // shipt sunk
        }

        void DrawAroundSunkShip(int index_i, int index_j, int fromWhichCalled_i = -1, int fromWhichCalled_j = -1)
        {
            // recursive function


            for (int i = -1; i <= 1; ++i)
            {
                if (index_i + i < 0 || index_i + i >= 10) //check for edge of field
                    continue;

                for (int j = -1; j <= 1; ++j)
                {

                    if (index_j + j < 0 || index_j + j >= 10) // check for edge of field
                        continue;

                    if (index_i + i == fromWhichCalled_i && index_j + j == fromWhichCalled_j) // in order not to cause infinite recursive call
                        continue;

                    if (j == 0 && i == 0) // exclude pair (0, 0)	
                        continue;


                    // placing "miss" around sunk ship
                    if (field[index_i + i, index_j + j] == CellType.Empty)
                    {
                        PlaceMiss(index_i + i, index_j + j);
                    }
                    else if (field[index_i + i, index_j + j] == CellType.HitShip)
                    {
                        DrawAroundSunkShip(index_i + i, index_j + j, index_i, index_j); // recursive call of function
                    }
                }
            }
        }
    }
}
