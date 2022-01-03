using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public static class AllSpells
    {
        public static Dictionary<string, Spell> data;
    }

    public static class SpellLoader
    {
        public static void LoadAllSpells(TextAsset json)
        {
            SpellImport spellImport = JsonUtility.FromJson<SpellImport>(json.text);
            AllSpells.data = spellImport.allSpells.ToDictionary(spell => spell.recipe, spell => spell);
        }
    }

    [Serializable]
    public class SpellImport
    {
        public List<Spell> allSpells;
    }

    [Serializable]
    public class Spell
    {
        public string recipe;
        public string color;
        public string name;
        public int totalCost;
        public List<V2Import> pattern;
        public int damage;
        public string spellEffect;
    }

    [Serializable]
    public class V2Import
    {
        public int x;
        public int y;
    }
}