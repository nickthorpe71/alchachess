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
                    while (IsInPattern(newSpell.Pattern, v2))
                    {
                        v2.x += (v2.x < 0) ? -1 : (v2.x > 0) ? 1 : 0;
                        v2.y += (v2.y < 0) ? -1 : (v2.y > 0) ? 1 : 0;
                    }
                    return v2;
                }).ToList();

                // add pattern and damage of component to spell
                newSpell.Pattern.AddRange(amplifiedPattern);
                newSpell = AddDamage(newSpell, component.Damage);
            }

            return newSpell;
        }

        public static Spell AddDamage(Spell spell, int dmgToAdd)
        {
            return new Spell(
                spell.Recipe,
                spell.Color,
                spell.Name,
                spell.TotalCost,
                spell.Pattern,
                spell.Damage + dmgToAdd
            );
        }

        private static bool IsInPattern(List<Vector2> pattern, Vector2 v2)
        {
            bool inPattern = false;
            for (int i = 0; i < pattern.Count; i++)
                if (pattern[i].x == v2.x && pattern[i].y == v2.y)
                    inPattern = true;
            return inPattern;
        }

        public static string SpellNameFromRecipe(string recipe, string spellColor)
        {
            if (NameLibrary.list.ContainsKey(recipe))
                return NameLibrary.list[recipe];

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

            NameLibrary.list[recipe] = name.Trim();
            return name.Trim();
        }

        public static string SpellEffectString(Spell spell, float power)
        {
            string txt = "";
            SpellEffect effect = SpellEffects.list[spell.Color];

            txt += $"A {SpellLetterToWord(spell.Color)} spell that:\n";

            if (effect.DamagesAllies)
                txt += $"- damages allies for {CalcDamage(spell.Damage, power)}\n";
            if (effect.DamagesEnemies)
                txt += $"- damages enemies for {CalcDamage(spell.Damage, power)}\n";
            if (effect.HealsAllies)
                txt += $"- heals allies for {CalcHeal(spell.Damage, power)}\n";
            if (effect.HealsEnemies)
                txt += $"- heals enemies for {CalcHeal(spell.Damage, power)}\n";
            if (effect.AltersEnvironment)
                txt += $"- creates {GetEnvironmentEffectAsStr(spell.Color)}";

            return txt;
        }

        public static string SpellLetterToWord(string spellColor)
        {
            string res = "";
            switch (spellColor)
            {
                case "G":
                    res = "green";
                    break;
                case "R":
                    res = "red";
                    break;
                case "D":
                    res = "black";
                    break;
                case "W":
                    res = "white";
                    break;
                case "B":
                    res = "blue";
                    break;
                case "Y":
                    res = "yellow";
                    break;
            }
            return res;
        }

        public static string GetEnvironmentEffectAsStr(string spellColor)
        {
            string res = "";
            switch (spellColor)
            {
                case "G":
                    res = "groves of vines that block movement";
                    break;
                case "R":
                    res = "flames which burn pieces that move over them";
                    break;
                case "D":
                    res = null;
                    break;
                case "W":
                    res = null;
                    break;
                case "B":
                    res = "ice spears that block movement";
                    break;
                case "Y":
                    res = "boulders that block movement";
                    break;
            }
            return res;
        }

        public static float CalcDamage(float baseDmg, float power) => Mathf.Floor(baseDmg * power);

        public static float CalcHeal(float baseDmg, float power) => Mathf.Floor(baseDmg * power) * 2.5f;

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
    }
}

