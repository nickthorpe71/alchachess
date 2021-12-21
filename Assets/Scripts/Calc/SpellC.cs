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

            var allPerms = GeneralC.GetPermutations(recipe, recipe.Length).ToList();

            for (int i = 0; i < allPerms.Count; i++)
            {
                string perm = new string(allPerms[i].ToArray());
                if (AllSpells.data.ContainsKey(perm))
                    return AllSpells.data[perm];
            }

            return null;
        }
    }
}

