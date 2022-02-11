using System;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label)
            => String.Format("Pieces/{0}", Enum.GetName(typeof(PieceLabel), label));

        public static string PieceAsString(Piece piece)
        {
            return $"|Label: {piece.label} |Color: {piece.color} |Health: {piece.health} |MaxHealth: {piece.maxHealth} |Power: {piece.power} |Move: {piece.moveDistance} " + $"|Player: {piece.player}| Element: " + piece.element.ToString();
        }

        public static Piece ApplySpellToPiece(Piece attacker, Piece defender, Spell spell)
        {
            Piece tileCopy = defender.Clone();
            float colorMod = SpellC.ColorMod(attacker.element, tileCopy.element, spell.color);
            SpellEffect spellEffect = SpellEffects.list[spell.color];
            bool isEnemy = attacker.player != defender.player;

            if (isEnemy)
            {
                if (spellEffect.DamagesEnemies)
                    tileCopy.health -= SpellC.CalcDamage(spell.damage, attacker.power, colorMod);
                if (spellEffect.HealsEnemies)
                    tileCopy.health += SpellC.CalcHeal(spell.damage, attacker.power, colorMod);
            }
            else // it's an ally
            {
                if (spellEffect.DamagesAllies)
                    tileCopy.health -= SpellC.CalcDamage(spell.damage, attacker.power, colorMod);
                if (spellEffect.HealsAllies)
                    tileCopy.health += SpellC.CalcHeal(spell.damage, attacker.power, colorMod);
            }

            if (tileCopy.health > tileCopy.maxHealth)
                tileCopy.health = tileCopy.maxHealth;

            if (tileCopy.health < 0)
                tileCopy.health = 0;

            return tileCopy;
        }
    }
}
