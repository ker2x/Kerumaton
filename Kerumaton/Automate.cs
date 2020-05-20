using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Kerumaton
{
    class Automate
    {
        Random rand = new Random();
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

        public enum Direction { N = 0,E = 1, S = 2, W = 3}
        public int id { get; set; }
        public int hp { get; set; }
        public bool isAlive { get; set; }
        public Color color { get; set; }
        public int energy { get; set; }
        public int lifetime { get; set; }



        public Automate(int x, int y, int id)
        {
            this.pos = new Position(x, y);
            this.id = id;
            color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
        }

        public void SampleTick(List<Automate> bots)
        {
            double closestDistance = int.MaxValue;
            Automate.Direction closestDir = this.RandomDirection();
            Automate.Direction tmpDirx, tmpDiry;

            foreach (var b in bots)
            {
                double distance = Math.Sqrt(
                    (b.pos.x - this.pos.x) * (b.pos.x - this.pos.x) + (b.pos.y - this.pos.y) * (b.pos.y - this.pos.y));

                if (distance < closestDistance && b.id != this.id)
                {
                    tmpDirx = (this.pos.x < b.pos.x) ? Automate.Direction.W : Automate.Direction.E;
                    tmpDiry = (this.pos.y < b.pos.y) ? Automate.Direction.N : Automate.Direction.S;
                    closestDir = (Math.Abs(b.pos.x - this.pos.x) < Math.Abs(b.pos.y - this.pos.y)) ? tmpDirx : tmpDiry;
                    closestDistance = distance;
                }
            }
            if (rand.Next(10000) == 1) { this.pos.y = rand.Next(MainWindow.imageHeight); this.pos.x = rand.Next(MainWindow.imageWidth); }
            //if (rand.Next(1000) == 0) { this.Move(this.RandomDirection()); }
            else
            this.Move(closestDir);

        }

        public Direction RandomDirection()
        {
            return (Direction)rand.Next(Enum.GetNames(typeof(Direction)).Length);
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
                        if (pos.y == MainWindow.imageHeight - 1) return false;
                        else
                        {
                            pos.y++;
                            return true;
                        }
                    }
                case Direction.W:
                    {
                        if (pos.x == 0) return false;
                        else
                        {
                            pos.x--;
                            return true;
                        }
                    }
                case Direction.E:
                    {
                        if (pos.x == MainWindow.imageWidth - 1) return false;
                        else
                        {
                            pos.x++;
                            return true;
                        }
                    }
                default:
                    return false;
            }
        }

    }
}
