using System;

namespace Kerumaton
{
    internal class WorldGrid
    {
        public const int gridWidth = 10;
        public const int gridHeight = 10;
        public int gridSizeX, gridSizeY;
        public int[][] gridArray;
        public WorldGrid()
        {
            gridSizeX = world.imageWidth % gridWidth;
            gridSizeY = world.imageHeight % gridHeight;
            gridArray = new int[gridSizeX][];
            for(int i = 0; i < gridArray.Length; i++)
            {
                gridArray[i] = new int[gridSizeY];
            }
        }
    }
}