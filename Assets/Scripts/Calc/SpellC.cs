using System.Linq;
using UnityEngine;
using Data;

namespace Calc
{
    public static class SpellC
    {
        public static Spell GetSpellByRecipe(string recipe)
        {
            if (recipe == "")
                return null;

            var allPerms = GeneralC.GetPermutations(recipe, recipe.Length).ToList();

            for (int i = 0; i < allPerms.Count; i++)
            {
                string perm = new string(allPerms[i].ToArray());
                if (AllSpells.data.ContainsKey(perm))
                    return AllSpells.data[perm];
            }

            return null;
        }

        public static string SpellEffectString(string effect, float damage, float power, float colorMod)
        {
            switch (effect)
            {
                case "heal":
                    return $"Heal your pieces in range for {CalcHeal(damage, power, colorMod)} HP.";
                case "burn":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and an additional {CalcBurn(CalcDamage(damage, power, colorMod))} damage for 3 turns.";
                case "poison":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and an additional {CalcPoison(CalcDamage(damage, power, colorMod))} damage for 3 turns.";
                case "frozen":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and can't move for 3 turns.";
                case "increase power":
                    return $"Your pieces in range have their power permanently increased by {CalcIncreasePower(damage, power, colorMod)}.";
                case "decrease power":
                    return $"Opponent's pieces in range have their power permanently decreased by {CalcDecreasePower(damage, power, colorMod)}.";
                default:
                    return $"Deal {CalcDamage(damage, power, colorMod)} to opponent's pieces in range.";
            }
        }

        // Status effect calculations
        public static float CalcHeal(float baseDmg, float power, float colorMod) => baseDmg * power * colorMod;
        public static float CalcBurn(float damage) => (damage / 10) * 2;
        public static float CalcPoison(float damage) => (damage / 10) * 2;
        public static float CalcIncreasePower(float baseDmg, float power, float colorMod) => baseDmg / 1000 * power * colorMod;
        public static float CalcDecreasePower(float baseDmg, float power, float colorMod) => baseDmg / 1000 * power * colorMod;
        public static float CalcDamage(float baseDmg, float power, float colorMod) => baseDmg * power * colorMod;

        public static int DetermineEffectTurns(string effect, float colorMod, int currentTurns) => (effect == "burn" || effect == "poison" || effect == "frozen") ? 3 + (int)Mathf.Floor(colorMod) : currentTurns;
        public static string DetermineLastingEffect(string effect) => (effect == "burn" || effect == "poison" || effect == "frozen") ? effect : "";

        public static string ColorToString(string color)
        {
            switch (color)
            {
                case "D":
                    return "Black";
                case "W":
                    return "White";
                case "B":
                    return "Blue";
                case "R":
                    return "Red";
                case "G":
                    return "Green";
                case "Y":
                    return "Yellow";
                default:
                    return "";
            }
        }

        public static float ColorMod(string attackerColor, string defenderColor, string spellColor)
            => (spellColor == attackerColor) ? 1.5f : (spellColor == ElementOpposites.list[defenderColor]) ? 0.5f : 1;
    }
}

