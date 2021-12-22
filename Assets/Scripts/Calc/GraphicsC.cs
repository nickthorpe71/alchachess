using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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

        public static GameObject GetPieceByPosition(List<GameObject> pieces, Vector2 position)
            => pieces.Where(piece => new Vector2(piece.transform.position.x, piece.transform.position.z) == position).ToList<GameObject>()[0];
    }
}
