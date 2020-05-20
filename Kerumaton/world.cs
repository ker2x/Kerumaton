using System;
using System.Collections.Generic;
using System.Text;

namespace Kerumaton
{
    static class World
    {
        //Parameters
        static public int maxAutomaton = 1000;
        static public int maxLifetime = 10000;
        
        //Enums
        public enum Direction { N = 0, E = 1, S = 2, W = 3 }

        public enum worldElement
        {
            EMPTY = 0,
            AUTOMATE = 1,
            WORLDBORDER = 2,
            ROCK = 3,
            LAVA = 4,
            WATER = 5,
            FOOD = 6
        };

        
    }
}
