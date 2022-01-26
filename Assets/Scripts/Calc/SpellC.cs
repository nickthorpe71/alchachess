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

            Spell newSpell = new Spell(recipe, color, SpellNameFromRecipe(recipe, color), recipe.Length, new List<Vector2>(), 0);

            for (int i = 0; i < recipe.Length; i++)
            {
                // convert char to string
                string element = "" + recipe[i];
                // get component by recipe element
                ElementalComponent component = ElementalComponents.list[element];

                // stack pattern on existing pattern
                List<Vector2> amplifiedPattern = component.Pattern.Select(v2 =>
                {
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

            return newSpell;
        }

        private static bool IsInPattern(List<Vector2> pattern, Vector2 v2)
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
            int nameLength = Random.Range(2, Mathf.Min(recipe.Length + 1, 5));

            // make sure there are enough elements to map to words
            while (inUseRecipe.Length < nameLength)
                inUseRecipe += spellColor;

            // select name pattern
            List<string> namePattern = GeneralC.RandomFromList(NamePatterns.list[nameLength]);

            for (int i = 0; i < nameLength; i++)
            {
                string nextWord = "";
                if (namePattern[i] == "being's")
                    nextWord = GeneralC.RandomFromList(ElementWords.list[inUseRecipe[i] + "-" + "being"]) + "'s";
                else
                    nextWord = GeneralC.RandomFromList(ElementWords.list[inUseRecipe[i] + "-" + namePattern[i]]);

                // add random word to name using next element in recipe and next type in pattern
                name += " " + GeneralC.CapitalizeFirstLetter(nextWord);
            }

            return name.Trim();
        }

        public static string SpellEffectString(float damage, float power, float colorMod) => $"Deal {CalcDamage(damage, power, colorMod)} to opponent's pieces in range.";

        public static float CalcDamage(float baseDmg, float power, float colorMod) => Mathf.Floor(baseDmg * power * colorMod);

        public static float CalcHeal(float baseDmg, float power, float colorMod) => Mathf.Floor(baseDmg * power * colorMod) * 3;

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

