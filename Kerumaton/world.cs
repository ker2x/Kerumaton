using System;
using System.Collections.Generic;
using System.Threading;

namespace Kerumaton
{
    internal static class World
    {
        //Parameters
        public const int maxAutomaton = 500;
        public const int maxLifetime = 100000;
        public const int imageWidth = 500;
        public const int imageHeight = 500;

        static public int lifetime = 0;
        static public int automatonLastID = 0;
        static public List<Automate> bots;

        //Enums
        public enum Direction { N = 0, E = 1, S = 2, W = 3 }

        public enum WorldElement
        {
            EMPTY = 0,
            AUTOMATE = 1,
            WORLDBORDER = 2,
            ROCK = 3,
            LAVA = 4,
            WATER = 5,
            FOOD = 6
        };

        public static void Tick()
        {
            lifetime++;
        }

        public static void SpawnAutomaton()
        {
            Random rand = new Random();
            Interlocked.Increment(ref automatonLastID);
            lock (bots)
            {
                bots.Add(new Automate(
                    rand.Next(0, imageWidth - 1),
                    rand.Next(0, imageHeight - 1),
                    automatonLastID)
                    );
            }
        }
    }
}