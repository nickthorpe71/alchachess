using UnityEngine;
using System;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label)
            => String.Format("Pieces/{0}", Enum.GetName(typeof(PieceLabel), label));

        public static Tile[][] UpdatePieceOnTile(Tile[][] tiles, Vector2 position, Piece piece)
        {
            Tile[][] tilesCopy = BoardC.MapTiles(tiles, (tile) => tile.Clone());
            tilesCopy[(int)position.y][(int)position.x].piece = piece;
            return tilesCopy;
        }

        public static string PieceAsString(Piece piece)
        {
            return $"|Label: {piece.label} |Color: {piece.color} |Health: {piece.health} |MaxHealth: {piece.maxHealth} |Power: {piece.power} |Move: {piece.moveDistance} " + $"|Player: {piece.player}| Element: " + piece.element.ToString();
        }

        public static Piece ApplySpellToPiece(Piece attacker, Piece defender, Spell spell)
        {
            Piece tileCopy = defender.Clone();
            float colorMod = SpellC.ColorMod(attacker.element, tileCopy.element, spell.color);
            tileCopy.health -= SpellC.CalcDamage(spell.damage, attacker.power, colorMod);
            return tileCopy;
        }
    }
}
