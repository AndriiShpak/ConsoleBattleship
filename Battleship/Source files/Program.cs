using System;

namespace Battleship
{
    public struct Pair<TFirst, TSecond>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
    }

    class Program
    {
        static void Main()
        {
            CoolConsoleStuff();

            IView view = new GreetingsView();

            while (view != null)
            {
                view.Draw();
                
                view = view.Handle();
            }
        }

        static void CoolConsoleStuff()
        {
            Console.WindowWidth = 80;
            Console.WindowHeight = 50;
            Console.BufferWidth = 80;
            Console.BufferHeight = 50;

            Console.CursorVisible = false;
        }
    }
}
