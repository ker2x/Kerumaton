using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Kerumaton;
using static Kerumaton.World;
using System.Threading.Tasks;

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

        //TYPE OF ACTOR TO BE EVENTUALLY IMPLEMENTED : 
        // - Something to move
        // - RADAR (beam, high range)
        // - EYE (large angle, low range)
        // - MOUTH (to eat)
        // - Element type identifier ? (food, automata, wall)
        // - Own Life sensor
        // - own feed level sensor
        // - world position sensor
        // - sex ?

        public int id { get; set; }
        public int hp { get; set; }
        public bool isAlive { get; set; }
        public Color color { get; set; }
        public int energy { get; set; }
        public int lifetime { get; set; }

        public worldElement ElementType = worldElement.AUTOMATE;

        public Automate(int x, int y, int id)
        {
            this.pos = new Position(x, y);
            this.id = id;
            color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            lifetime = rand.Next(maxLifetime);
        }

        public void SampleTick(List<Automate> bots)
        {
            Direction closestDir = this.findClosestDirection(bots);

            if (lifetime >= World.maxLifetime) { 
                this.pos.y = rand.Next(MainWindow.imageHeight); 
                this.pos.x = rand.Next(MainWindow.imageWidth);
                lifetime = rand.Next(maxLifetime);
            }
            else if (rand.Next(1000) == 0) this.Move(this.RandomDirection());
            else this.Move(closestDir);

        }

        Direction findClosestDirection(List<Automate> bots)
        {
            double closestDistance = int.MaxValue;
            Direction closestDir = this.RandomDirection();
            Direction tmpDirx, tmpDiry;
            lifetime++;

            foreach (var b in bots)
            //Parallel.ForEach( bots, b =>
            {
                double distance = Math.Sqrt(
                    (b.pos.x - this.pos.x) * (b.pos.x - this.pos.x) + (b.pos.y - this.pos.y) * (b.pos.y - this.pos.y));

                if (distance < closestDistance && b.id != this.id)
                {
                    tmpDirx = (this.pos.x < b.pos.x) ? Direction.W : Direction.E;
                    tmpDiry = (this.pos.y < b.pos.y) ? Direction.N : Direction.S;
                    closestDir = (Math.Abs(b.pos.x - this.pos.x) < Math.Abs(b.pos.y - this.pos.y)) ? tmpDirx : tmpDiry;
                    lock ((object)closestDistance) { closestDistance = distance; }
                }
            }
            //});
            return closestDir;

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
