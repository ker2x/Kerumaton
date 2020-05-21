using System;
using System.Windows.Media;
using static Kerumaton.World;

namespace Kerumaton
{
    internal class Automate
    {
        private Random rand = new Random();

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

        public long ID { get; set; }
        public int HP { get; set; }
        public bool IsAlive { get; set; }
        public Color Color { get; set; }
        public int Energy { get; set; }
        public int Lifetime { get; set; }

        public WorldElement ElementType = WorldElement.AUTOMATE;

        public Automate(int x, int y, long id)
        {
            this.pos = new Position(x, y);
            this.ID = id;
            Color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            Lifetime = rand.Next(maxLifetime);
        }

        public void SampleTick()
        {
            Lifetime++;

            Direction closestDir = this.FindClosestDirection();

            if (Lifetime >= World.maxLifetime)
            {
                this.pos.y = rand.Next(MainWindow.imageHeight);
                this.pos.x = rand.Next(MainWindow.imageWidth);
                Lifetime = rand.Next(maxLifetime);
            }
            else if (rand.Next(1000) == 0) this.Move(this.RandomDirection());
            else this.Move(closestDir);
        }

        private Automate FindNearestNeighbor()
        {
            float distance;
            float closestDistance = float.MaxValue;
            Automate found = null;
            foreach (var bot in World.bots)
            {
                //Math.Sqsrt isn't required if we don't really need the actual distance
                distance = (bot.pos.x - this.pos.x) * (bot.pos.x - this.pos.x)
                         + (bot.pos.y - this.pos.y) * (bot.pos.y - this.pos.y);

                //We need to ignore "self" otherwise the closest distance is always 0.0 ;)
                if (distance < closestDistance && bot.ID != this.ID)
                {
                    closestDistance = distance;
                    found = bot;
                }
            }
            return found;
        }

        private Direction FindClosestDirection()
        {
            Automate closest = FindNearestNeighbor();
            Direction tmpDirx = (this.pos.x < closest.pos.x) ? Direction.W : Direction.E;
            Direction tmpDiry = (this.pos.y < closest.pos.y) ? Direction.N : Direction.S;
            return (Math.Abs(closest.pos.x - this.pos.x) < Math.Abs(closest.pos.y - this.pos.y)) ? tmpDirx : tmpDiry;
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