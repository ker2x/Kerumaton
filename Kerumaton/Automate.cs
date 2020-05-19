using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Kerumaton
{
    class Automate
    {
        // The automate position
        public class Position
        {
            public int x { get; set; }
            public int y { get; set; }

            public Position()
            {
                x = 0;
                y = 0;
            }

            public Position(int _x, int _y)
            {
                x = _x;
                y = _y;
            }

            public override string ToString() => $"({x}, {y})";
        }

        public Position pos;

        public enum Direction { N,NE,E,SE,S,SW,W,NW}
        public int id { get; set; }
        public int hp { get; set; }
        public bool isAlive { get; set; }
        public Colors color { get; set; }
        public int energy { get; set; }
        public int lifetime { get; set; }



        public Automate(int x, int y, int id)
        {
            pos = new Position(x, y);
            this.id = id;
        }

        public bool Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    {
                        if (pos.y == 0) return false;
                        else
                        {
                            pos.y--;
                            return true;
                        }
                    }
                case Direction.S:
                    {
                        if (pos.y == MainWindow.bmp.PixelHeight - 1) return false;
                        else
                        {
                            pos.y++;
                            return true;
                        }
                    }
                default:
                    return false;
            }
        }

    }
}
