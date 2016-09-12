using System;

namespace Battleship
{
    interface IView
    {
        void Draw();

        IView Handle();
    }
}
