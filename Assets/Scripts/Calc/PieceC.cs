using UnityEngine;
using System;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label)
            => String.Format("Pieces/{0}", Enum.GetName(typeof(PieceLabel), label));

        public static int CalcExpForNextLevel(float currentLevel)
            => (int)Math.Floor(((Math.Pow(currentLevel, 4) + 10 * Math.Pow(currentLevel, 3) + 37 * Math.Pow(currentLevel, 2) + 57 * currentLevel - 96) / 16) * 100);

        public static int CalcExpFromDefeatingOther(float myLevel, float opponentLevel)
            => (int)Math.Floor(((Math.Pow(opponentLevel, 4) + 10 * Math.Pow(opponentLevel, 3) + 37 * Math.Pow(opponentLevel, 2) + 57 * opponentLevel - 96) / myLevel) * 5);

        public static Tile[][] UpdatePieceOnTile(Tile[][] tiles, Vector2 position, Piece piece)
        {
            Tile[][] tilesCopy = BoardC.MapTiles(tiles, (tile) => tile.Clone());
            tilesCopy[(int)position.y][(int)position.x].piece = piece;
            return tilesCopy;
        }

        public static string PieceAsString(Piece piece)
        {
            return $"|Label: {piece.label} |Color: {piece.color} |Health: {piece.health} |MaxHealth: {piece.maxHealth} |Level: {piece.level} |Exp: {piece.experience} |Attack: {piece.attack} |Move: {piece.moveDistance} |Effect: " + piece.currentSpellEffect + $"|Player: {piece.player}| Element: " + piece.element.ToString();
        }
    }
}
