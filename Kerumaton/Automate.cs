using System;
using System.Windows.Media;
using static Kerumaton.world;

namespace Kerumaton
{
    internal class Automate
    {
        private readonly Random rand = new Random();

        // The automate position
        public class Position
        {
            public int x;//{ get; set; }
            public int y;// { get; set; }

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
            pos = new Position(x, y);
            ID = id;
            Color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            Lifetime = rand.Next(maxLifetime);
        }

        public void Tick()
        {
            Lifetime++;

            if (Lifetime >= world.maxLifetime)
            {
                pos.y = rand.Next(world.imageHeight);
                pos.x = rand.Next(world.imageWidth);
                Lifetime = rand.Next(maxLifetime);
            }

        }

        public void SampleTick()
        {
            Lifetime++;

            Direction closestDir = FindClosestDirection();

            if (Lifetime >= world.maxLifetime)
            {
                pos.y = rand.Next(world.imageHeight);
                pos.x = rand.Next(world.imageWidth);
                Lifetime = rand.Next(maxLifetime);
            }
            //            else if (rand.Next(1000) == 0)
            //            {
            //                Move(RandomDirection());
            //            }
            else
            {
                Move(closestDir);
            }
        }

        private Automate FindNearestNeighbor()
        {
            float distance;
            float closestDistance = float.MaxValue;
            Automate found = null;
            foreach (var bot in world.bots)
            {
                //Math.Sqsrt isn't required if we don't really need the actual distance
                distance = (bot.pos.x - pos.x) * (bot.pos.x - pos.x)
                         + (bot.pos.y - pos.y) * (bot.pos.y - pos.y);

                //We need to ignore "self" otherwise the closest distance is always 0.0 ;)
                if (distance < closestDistance && distance != 0.0)
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
            if (pos.x == closest.pos.x && (pos.y == closest.pos.y))
            {
                return RandomDirection();
            }

            Direction tmpDirx = (pos.x < closest.pos.x) ? Direction.W : Direction.E;
            Direction tmpDiry = (pos.y < closest.pos.y) ? Direction.N : Direction.S;
            return (Math.Abs(closest.pos.x - pos.x) < Math.Abs(closest.pos.y - pos.y)) ? tmpDirx : tmpDiry;
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
                        if (pos.y == 0)
                        {
                            return false;
                        }
                        else
                        {
                            pos.y--;
                            return true;
                        }
                    }
                case Direction.S:
                    {
                        if (pos.y == world.imageHeight - 1)
                        {
                            return false;
                        }
                        else
                        {
                            pos.y++;
                            return true;
                        }
                    }
                case Direction.W:
                    {
                        if (pos.x == 0)
                        {
                            return false;
                        }
                        else
                        {
                            pos.x--;
                            return true;
                        }
                    }
                case Direction.E:
                    {
                        if (pos.x == world.imageWidth - 1)
                        {
                            return false;
                        }
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