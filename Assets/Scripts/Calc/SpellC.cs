using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Calc
{
    public static class SpellC
    {
        public static Spell GetSpellByRecipe(string recipe)
        {
            if (recipe == "")
                return null;

            // color of the spell is == to the last element 
            string color = recipe[recipe.Length - 1].ToString();

            Spell newSpell = new Spell(recipe, color, SpellNameFromRecipe(recipe, color), recipe.Length, new List<V2Import>(), 0, "");

            for (int i = 0; i < recipe.Length; i++)
            {
                // convert char to string
                string element = "" + recipe[i];
                // get component by recipe element
                ElementalComponent component = ElementalComponents.list[element];

                // stack pattern on existing pattern
                List<V2Import> amplifiedPattern = component.Pattern.Select(v2Import =>
                {
                    V2Import v2 = new V2Import(v2Import.x, v2Import.y);
                    while (IsInPattern(newSpell.pattern, v2))
                    {
                        v2.x += (v2.x < 0) ? -1 : (v2.x > 0) ? 1 : 0;
                        v2.y += (v2.y < 0) ? -1 : (v2.y > 0) ? 1 : 0;
                    }
                    return v2;
                }).ToList();

                // add pattern and damage of component to spell
                newSpell.pattern.AddRange(amplifiedPattern);
                newSpell.damage += component.Damage;
            }

            // add effect for spell color
            newSpell.spellEffect = ElementalComponents.list[color].Effect;
            return newSpell;

            // var allPerms = GeneralC.GetPermutations(recipe, recipe.Length).ToList();

            // for (int i = 0; i < allPerms.Count; i++)
            // {
            //     string perm = new string(allPerms[i].ToArray());
            //     if (AllSpells.data.ContainsKey(perm))
            //         return AllSpells.data[perm];
            // }

            // return null;
        }

        private static bool IsInPattern(List<V2Import> pattern, V2Import v2)
        {
            bool inPattern = false;
            for (int i = 0; i < pattern.Count; i++)
                if (pattern[i].x == v2.x && pattern[i].y == v2.y)
                    inPattern = true;
            return inPattern;
        }

        private static string SpellNameFromRecipe(string recipe, string spellColor)
        {
            string name = "";
            string inUseRecipe = recipe;
            int nameLength = Random.Range(2, Mathf.Min(recipe.Length + 1, 4));
            int charsLeft = nameLength;

            // make sure there are enough elements to map to words
            while (inUseRecipe.Length < nameLength)
                inUseRecipe += spellColor;

            string toAdd = "";
            // parts of words that should not be used as first word
            List<string> cantBeFirstWord = new List<string> { "of" };
            if (nameLength == 1) cantBeFirstWord.Add("'s");

            // add first word to match spell color
            do
            {
                toAdd = GeneralC.CapitalizeFirstLetter(GeneralC.RandomFromList(ElementWords.list[spellColor]));
            } while (cantBeFirstWord.Any(s => toAdd.Contains(s)));

            name += toAdd;
            // remove last char of inUseRecipe since it has been used
            charsLeft--;
            inUseRecipe = inUseRecipe.Substring(0, inUseRecipe.Length - 1);

            // add random words to name based on remaining recipe
            while (charsLeft > 0)
            {
                // select random element from recipe
                List<string> recipeAsList = inUseRecipe.ToCharArray().Select(c => "" + c).ToList();
                string nextElement = GeneralC.RandomFromList(recipeAsList);

                // get random word using nextElement
                toAdd = " " + GeneralC.CapitalizeFirstLetter(GeneralC.RandomFromList(ElementWords.list[nextElement]));

                // add to name and remove used element
                name += toAdd;
                charsLeft--;
                inUseRecipe.Remove(inUseRecipe.IndexOf(nextElement));
            }

            return name;
        }

        public static string SpellEffectString(string effect, float damage, float power, float colorMod)
        {
            switch (effect)
            {
                case "heal":
                    return $"Heal your pieces in range for {CalcHeal(damage, power, colorMod)} HP.";
                case "burn":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and an additional {CalcBurn(CalcDamage(damage, power, colorMod))} damage each upkeep for 3 turns.";
                case "poison":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and an additional {CalcPoison(CalcDamage(damage, power, colorMod))} damage each upkeep for 3 turns.";
                case "frozen":
                    return $"Opponent's pieces in range take {CalcDamage(damage, power, colorMod)} damage and can't move for 2 turns.";
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

