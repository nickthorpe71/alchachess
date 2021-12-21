using System;

namespace Calc
{
    public static class GraphicsC
    {
        public static void LoopTileGraphicsWithIndexes(TileGraphic[][] graphics, Action<TileGraphic, int, int> f)
        {
            for (int y = 0; y < graphics.Length; y++)
                for (int x = 0; x < graphics[y].Length; x++)
                    f(graphics[y][x], x, y);
        }
    }
}
