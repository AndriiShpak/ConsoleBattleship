using System;

namespace Battleship
{
    class GameView: BaseView
    {
        // game fields 
        GameField playerField = new GameField(4, 18, GameField.FieldType.PlayerField);
        GameField oponentField = new GameField(46, 18, GameField.FieldType.OponentField);

        // logic object
        GameLogic gameLogic = new GameLogic();
        
        // where to draw score
        static readonly int whereScoreStarts = 14;

        public override void Draw()
        {
            CleanUnderLogo();

            playerField.DrawField();
            oponentField.DrawField();

            RedrawScore();
        }

        public override IView Handle()
        {
            bool needToExit = false;

            while (true)
            {
                HandleShipPlacement(ref needToExit);

                if (needToExit)
                {
                    break;
                }

                if (HandleGame(ref needToExit))
                {
                    GameMessages.DrawHint(LanguageManipulator.GetHowToDrawWinHint);

                    Console.ReadKey();
                    GameMessages.ClearHints();

                    // if win than renew fields
                    
                    playerField.RenewField();
                    playerField.DrawField();

                    oponentField.RenewField();
                    oponentField.DrawField();
                }
                else
                {
                    // else exit the game

                    if (!needToExit)
                    {
                        GameMessages.DrawHint(LanguageManipulator.GetHowToDrawLoseHint);

                        Console.ReadKey();
                    }
                    
                    break;
                }     
            }

            GameMessages.ClearHints();

            return new ResultView(gameLogic.Score);
        }
        
        void HandleShipPlacement(ref bool needToExit)
        {
            GameMessages.DrawHint(LanguageManipulator.GetHowToDrawShipPlacementHint);

            for (int size = 4; size >= 1; --size) // ship size
            {
                for (int number = 1; number <= 5 - size; ++number) // number of ships
                {
                    HandleOneShip(size, ref needToExit);
                }
            }

            oponentField.RandomShipPlacement();
            GameMessages.ClearHints();  
        }

        void HandleOneShip(int size, ref bool needToExit)
        {
            bool isPlaced = false;
            
            // starting point of ship
            Point current = new Point { X = 0, Y = 0 };
            GameField.Direction direction = GameField.Direction.Horizontal;
            
            playerField.DrawNewShipPosition(current.X, current.Y, size, direction);
            
            // moves around field
            while (!isPlaced && !needToExit)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow: //key up
                        if (!IsOkayToMakeMove(current.X - 1, current.Y, size, direction))
                            break;

                        --current.X;
                        break;

                    case ConsoleKey.DownArrow: //key down
                        if (!IsOkayToMakeMove(current.X + 1, current.Y, size, direction))
                            break;

                        ++current.X;
                        break;

                    case ConsoleKey.LeftArrow: //key left
                        if (!IsOkayToMakeMove(current.X, current.Y - 1, size, direction))
                            break;

                        --current.Y;
                        break;

                    case ConsoleKey.RightArrow: //key right
                        if (!IsOkayToMakeMove(current.X, current.Y + 1, size, direction))
                            break;

                        ++current.Y;
                        break;

                    case ConsoleKey.Q: // Q key to rotate
                        if (direction == GameField.Direction.Horizontal)
                            direction = GameField.Direction.Vertical;
                        else
                            direction = GameField.Direction.Horizontal;

                        break;
                        
                    case ConsoleKey.Enter: // key enter
                        if (playerField.IsOkayToPlaceShip(current.X, current.Y, size, direction))
                            isPlaced = true;
                        break;

                    case ConsoleKey.Escape: // key escape
                        needToExit = true;
                        break;

                    default:
                        continue;
                }
                
                // renewing field
                if (isPlaced)
                {
                    playerField.PlaceShip(current.X, current.Y, size, direction);
                    playerField.DrawField();
                }
                else
                {
                    playerField.DrawField();
                    playerField.DrawNewShipPosition(current.X, current.Y, size, direction);
                }
            }
        }

        bool IsOkayToMakeMove(int index_i, int index_j)
        {
            // whether not going out of field

            if (index_i < 0 || index_i > 9 || index_j < 0 || index_j > 9)
                return false;

            return true;
        }

        bool IsOkayToMakeMove(int index_i, int index_j, int size, GameField.Direction shipDirection)
        {
            // check is okay to move ship in cells

            if (shipDirection == GameField.Direction.Horizontal)
            {
                for (int k = 0; k < size; ++k)
                {
                    if (!IsOkayToMakeMove(index_i, index_j + k))
                        return false;
                }
            }
            else
            {
                for (int k = 0; k < size; ++k)
                {
                    if (!IsOkayToMakeMove(index_i + k, index_j))
                        return false;
                }
            }

            return true;
        }

        bool HandleGame(ref bool needToExit)
        {
            bool isLose = false;
            bool isWin = false;
            Point current = new Point { X = 0, Y = 0 };

            oponentField.DrawSelectedCell(current.X, current.Y);

            // handling game
            while (!isLose && !isWin)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow: //key up
                        if (!IsOkayToMakeMove(current.X - 1, current.Y))
                            break;

                        --current.X;
                        break;

                    case ConsoleKey.DownArrow: //key down
                        if (!IsOkayToMakeMove(current.X + 1, current.Y))
                            break;

                        ++current.X;
                        break;

                    case ConsoleKey.LeftArrow: //key left
                        if (!IsOkayToMakeMove(current.X, current.Y - 1))
                            break;

                        --current.Y;
                        break;

                    case ConsoleKey.RightArrow: //key right
                        if (!IsOkayToMakeMove(current.X, current.Y + 1))
                            break;

                        ++current.Y;
                        break;

                    case ConsoleKey.Enter: // key enter
                        if (!gameLogic.HandlePlayerChoice(oponentField, current.X, current.Y, ref isWin))
                        {
                            // if hit or bad choice than player choose again
                            // also when win than break
                            break;
                        }

                        while (gameLogic.MakeOponentChoice(playerField, ref isLose)) // while oponent hit
                        {/*empty block*/}
                        
                        break;

                    case ConsoleKey.Escape:
                        needToExit = true;
                        isLose = true;
                        break;

                    default:
                        break;
                }

                // refresh field after move
                oponentField.DrawField();
                oponentField.DrawSelectedCell(current.X, current.Y);
                RedrawScore();
            }
            
            return isWin;
        }

        void RedrawScore()
        {
            Console.SetCursorPosition(0, whereScoreStarts);

            ConsoleHelper.PrintCentered(string.Format("   Score: {0}", gameLogic.Score), ConsoleColor.DarkGray);
        }
    }
}
