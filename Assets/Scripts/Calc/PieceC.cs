using UnityEngine;
using System;
using System.Collections.Generic;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label)
            => String.Format("Pieces/{0}", Enum.GetName(typeof(PieceLabel), label));

        public static int ExpForNextLevel(float currentLevel)
            => (int)Math.Floor(((Math.Pow(currentLevel, 4) + 10 * Math.Pow(currentLevel, 3) + 37 * Math.Pow(currentLevel, 2) + 57 * currentLevel - 96) / 16) * 100);

        public static int ExpFromDefeatingOther(float myLevel, float opponentLevel)
            => (int)Math.Floor(((Math.Pow(opponentLevel, 4) + 10 * Math.Pow(opponentLevel, 3) + 37 * Math.Pow(opponentLevel, 2) + 57 * opponentLevel - 96) / myLevel) * 5);

        public static float ExpAsPercent(float exp, float level)
        {
            float prevExpCap = level == 1 ? 0 : ExpForNextLevel(level - 1);
            float currentExpCap = ExpForNextLevel(level);
            return (prevExpCap - exp) / (currentExpCap - prevExpCap);
        }

        public static float CalcLevelFromExp(float exp)
        {
            if (exp < ExpForNextLevel(1))
                return 1;

            float currentExp = exp;
            float resultingLevel = 1;

            while (currentExp > 0)
            {
                currentExp -= ExpForNextLevel(resultingLevel);
                resultingLevel++;
            }

            return resultingLevel;
        }

        public static Tile[][] UpdatePieceOnTile(Tile[][] tiles, Vector2 position, Piece piece)
        {
            Tile[][] tilesCopy = BoardC.MapTiles(tiles, (tile) => tile.Clone());
            tilesCopy[(int)position.y][(int)position.x].piece = piece;
            return tilesCopy;
        }

        public static string PieceAsString(Piece piece)
        {
            return $"|Label: {piece.label} |Color: {piece.color} |Health: {piece.health} |MaxHealth: {piece.maxHealth} |Level: {piece.level} |Exp: {piece.experience} |Power: {piece.power} |Move: {piece.moveDistance} |Effect: " + piece.currentSpellEffect + $"|Player: {piece.player}| Element: " + piece.element.ToString();
        }

        public static float HealthAdjust(float damage, float power, string effect, float colorMod)
        {
            if (effect == "increase power" || effect == "decrease power")
                return 0;

            return (effect == "heal") ? SpellC.CalcHeal(damage, power, colorMod) : SpellC.CalcDamage(damage, power, colorMod) * -1;
        }

        public static float PowerAdjust(float damage, float power, string effect, float colorMod)
        {
            if (effect == "increase power")
                return SpellC.CalcIncreasePower(damage, power, colorMod);

            if (effect == "decrease power")
                return SpellC.CalcDecreasePower(damage, power, colorMod) * -1;

            return 0;
        }

        public static Dictionary<Vector2, StatusChange> GetCurrentStatusEffects(Tile[][] tiles)
        {
            Dictionary<Vector2, StatusChange> result = new Dictionary<Vector2, StatusChange>();

            BoardC.LoopTiles(tiles, tile =>
            {
                if (tile.contents != TileContents.Piece) return;

                if (tile.piece.currentSpellEffect == "" || tile.piece.currentSpellEffect == null || tile.piece.currentSpellEffect == "none")
                    return;

                result[new Vector2(tile.x, tile.y)] = new StatusChange(
                    tile.piece.effectDamage,
                    tile.piece.currentSpellEffect,
                    tile.piece.effectInflictor);
            });
            return result;
        }

        public static Tile ApplyStatusEffects(Tile tileCopy, StatusChange statusChange)
        {
            tileCopy.piece.effectTurnsLeft = tileCopy.piece.effectTurnsLeft > 1 ? tileCopy.piece.effectTurnsLeft - 1 : 0;
            tileCopy.piece.health -= statusChange.damage;
            if (tileCopy.piece.effectTurnsLeft <= 0)
                tileCopy.piece.currentSpellEffect = "";
            return tileCopy;
        }

        public static Piece ApplySpellToPiece(Piece attacker, Piece defender, Spell spell)
        {
            Piece tileCopy = defender.Clone();
            float colorMod = SpellC.ColorMod(attacker.element, tileCopy.element, spell.color);
            tileCopy.health += HealthAdjust(spell.damage, attacker.power, spell.spellEffect, colorMod);
            tileCopy.power += PowerAdjust(spell.damage, attacker.power, spell.spellEffect, colorMod);
            tileCopy.currentSpellEffect = SpellC.DetermineLastingEffect(spell.spellEffect);
            tileCopy.effectTurnsLeft = SpellC.DetermineEffectTurns(spell.spellEffect, colorMod, tileCopy.effectTurnsLeft);
            tileCopy.effectDamage
                = spell.spellEffect == "burn" ? SpellC.CalcBurn(SpellC.CalcDamage(spell.damage, attacker.power, colorMod))
                : spell.spellEffect == "poison" ? SpellC.CalcPoison(SpellC.CalcDamage(spell.damage, attacker.power, colorMod))
                : 0;
            tileCopy.effectInflictor = attacker.label;

            return tileCopy;
        }
    }
}
