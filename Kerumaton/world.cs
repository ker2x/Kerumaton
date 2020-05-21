using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Kerumaton
{
    internal static class World
    {
        //Parameters
        public const int maxAutomaton = 1000;

        public const int maxLifetime = 100000;
        static public Int64 automatonLastID = 0;
        static public ConcurrentBag<Automate> bots;

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

        public static void SpawnAutomaton()
        {
            Random rand = new Random();
            Interlocked.Increment(ref automatonLastID);
            bots.Add(new Automate(rand.Next(300, 700), rand.Next(300, 700), automatonLastID));
        }
    }
}