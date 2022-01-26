using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Data;

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
        {
            List<GameObject> result = pieces.Where(piece => new Vector2(piece.transform.position.x, piece.transform.position.z) == position).ToList<GameObject>();
            return (result.Count > 0) ? result[0] : null;
        }

        public static string GetSpellAnimPrefabPath(Spell spell) => "SpellAnims/" + spell.color + "SpellAnim";

        public static string GetCastAnimPrefabPath(Spell spell) => "SpellAnims/CastAnims/" + spell.color + "CastAnim";

        public static string GetEnvironmentPrefabPath(Spell spell) => "EnvironmentEffects/" + spell.color;

        public static PieceStats GetPieceStatsUI(Vector2 piecePos, List<GameObject> activePieces)
        {
            GameObject pieceGraphic = GraphicsC.GetPieceByPosition(activePieces, piecePos);
            return pieceGraphic.GetComponentInChildren<PieceStats>();
        }
    }
}
