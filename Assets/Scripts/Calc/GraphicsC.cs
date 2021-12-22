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
            => pieces.Where(piece => new Vector2(piece.transform.position.x, piece.transform.position.z) == position).ToList<GameObject>()[0];

        public static string GetSpellAnimPrefabPath(Spell spell)
        {
            //TODO: create animations for all spells once they are finalized
            // then use spell name to get correct animation name

            // This is temporary while we are still developing spells
            return "SpellAnims/" + spell.color + "SpellAnim";
        }

        public static string GetCastAnimPrefabPath(Spell spell) => "SpellAnims/CastAnims/" + spell.color + "CastAnim";
    }
}
