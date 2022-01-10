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

    public static class ElementOpposites
    {
        public static Dictionary<string, string> list = new Dictionary<string, string>{
            {"D", "W"},
            {"W", "D"},
            {"R", "B"},
            {"B", "R"},
            {"Y", "G"},
            {"G", "Y"},
            {"N", "N"}
        };
    }

    public static class ElementalComponents
    {
        public static Dictionary<string, ElementalComponent> list = new Dictionary<string, ElementalComponent>{
            {"D", new ElementalComponent("D", new List<V2Import>{new V2Import(1, -1), new V2Import(-1, -1)}, 152, "")},
            {"W", new ElementalComponent("W", new List<V2Import>{new V2Import(1, 0), new V2Import(-1, 0), new V2Import(0, -1), new V2Import(0, 1), new V2Import(1, 1), new V2Import(-1, 1), new V2Import(1, -1), new V2Import(-1, -1)}, 50, "heal")},
            {"R", new ElementalComponent("R", new List<V2Import>{new V2Import(0, 1),new V2Import(0, 2)}, 125, "burn")},
            {"B", new ElementalComponent("B", new List<V2Import>{new V2Import(1, 0), new V2Import(-1, 0)}, 75, "frozen")},
            {"Y", new ElementalComponent("Y", new List<V2Import>{new V2Import(1, 0), new V2Import(-1, 0), new V2Import(0, -1), new V2Import(0, 1)}, 100, "")},
            {"G", new ElementalComponent("G", new List<V2Import>{new V2Import(1, 1), new V2Import(-1, 1), new V2Import(1, -1), new V2Import(-1, -1)}, 76, "poison")}
        };
    }

    public class ElementalComponent
    {
        private readonly string _element;
        private readonly List<V2Import> _pattern;
        private readonly int _damage;
        private readonly string _effect;

        public ElementalComponent(string element, List<V2Import> pattern, int damage, string effect)
        {
            _element = element;
            _pattern = pattern;
            _damage = damage;
            _effect = effect;
        }

        public string Element
        {
            get { return _element; }
        }

        public List<V2Import> Pattern
        {
            get { return _pattern; }
        }

        public int Damage
        {
            get { return _damage; }
        }

        public string Effect
        {
            get { return _effect; }
        }
    }

    // what is needed are lists of adjectives, verbs, beings, and ofs
    // then patterns that make sense for each
    // ex:
    // - adjective + being 
    // - adjective + being + of
    // - adjective + thing 
    // - adjective + thing + of
    // - adjective + being's + thing
    // - adjective + adjective + being
    // - adjective + adjective + being + of
    // - adjective + verb + being
    // - adjective + verb + being + of
    // - verb + being
    // - verb + thing
    // - verb + being + thing
    // - verb + being + of
    // - verb + adjective + being
    // - verb + adjective + being + of
    // - being's + thing
    // - being's + adjective + thing
    // - being's + adjective + thing + of
    // - being's + adjective + being
    // - being's + adjective + adjective + being
    // - being's + adjective + being + of
    // - thing + being
    // - thing + being's + thing 
    // - thing + being's + thing + of
    // - thing + being's + adjective + thing
    // - thing + being's + adjective + thing + of

    public class ElementWords
    {
        public static Dictionary<string, List<string>> list = new Dictionary<string, List<string>>{
            {"D-adjective", new List<string>{
                "cursed", "shadowy", "shady", "black", "bony", "bone", "gloomy", "fleshy", "bloody", "ghostly", "haunted", "necro", "gruesome", "sinister", "voodoo", "putrid", "dark", "demi", "dimensional", "fearful", "horrible", "demonic", "wrathful", "scornful", "deceitful"
            }},
            {"D-verb", new List<string>{
                "phasing", "screaming", "choking", "staring", "puking", "shuddering", "gouging", "biting", "sneaking", "draining", "haunting", "bleeding", "horrifying"
            }},
            {"D-thing", new List<string>{
                "touch", "phase", "stab", "curse", "stare", "puke", "gouge", "bite", "promise", "scream", "hex", "dimension", "gloom", "fear", "flesh", "blood", "gravity", "torture", "ritual", "disguise", "demon", "contract", "deal", "horror", "exorcism", "moonlight", "moon", "abyss", "pit", "grime", "stench", "skull",
            }},
            {"D-being", new List<string>{
                "shade", "thief", "traitor", "wraith", "phantom", "ghost", "spectre", "demon", "gargoyle", "witch", "warlock", "exorcist", "geist", "hangman", "banshee", "ghoul"
            }},
            {"D-of", new List<string>{
                "of Darkness", "of the Shade", "of Blood", "of the Moon", "of the Pit", "of the Witch", "of Horror"
            }},
            {"W-adjective", new List<string>{
                "light", "holy", "divine", "hopeful", "faithful", "pure", "white", "pius", "angelic", "radiant"
            }},
            {"W-verb", new List<string>{
                "healing", "praying", "singing", "fading", "blinding", "glaring", "beaming", "consuming", "protecting", "sailing", "soaring", "flying",
                "gliding",
            }},
            {"W-thing", new List<string>{
                "light", "heal", "cure", "prayer", "serenity", "flash", "hymn", "song", "spirit", "soul", "hope", "faith", "smite", "redemption",
                "psalm", "knowledge", "lore", "enigma", "wings", "paradise", "beam", "plasma", "tradition", "photon", "breeze", "choice", "judgement",  "ray", "conviction", "game", "horn", "prison", "birth"
            }},
            {"W-being", new List<string>{
                "eden", "angel", "saint", "priest", "nun", "humanity", "spirit", "father", "mother"
            }},
            {"W-of", new List<string>{
                "of Eden", "of Light", "of Healing", "of Legend", "of Paradise", "of the Clouds", "of the Wind"
            }},
            {"R-adjective", new List<string>{
                "scorched", "burnt", "hellish", "fiery", "seared", "red", "enraged"
            }},
            {"R-verb", new List<string>{
                "burning", "flaming", "blazing", "blasting","searing", "sizzling", "immolating", "booming", "scalding", "raging", "cascading", "consuming",
            }},
            {"R-thing", new List<string>{
                "scorcher", "hell", "flame", "fireball", "fire", "combust", "lava", "blast", "breath", "sun", "prism", "inferno", "pyroblast", "flamestrike", "strike", "hellfire", "magma", "phoenix", "sizzle", "dragon", "flash", "bang", "boom", "magus", "wall", "link", "chasm", "fuel", "bull", "tiger", "cascade", "spear", "attack", "roar", "chant", "meteor", "berserk"
            }},
            {"R-being", new List<string>{
                "dragon", "hell", "phoenix", "tiger", "mage", "bull", "lizard", "salamander", "berserker", "red Dragon"
            }},
            {"R-of", new List<string>{
                "of Flame", "of the Sun", "of Hellfire", "of the Tiger", "of the Lizard", "of the Berserker", "of the Dragon", "of the Salamander", "of the Bull"
            }},
            {"B-adjective", new List<string>{
                "glacial", "aqua", "charming", "wet", "cold", "frozen", "shapeless", "glass", "blue", "mystical", "legendary", "forgotten", "invisible", "elegant", "lavishing", "romantic", "drunken", "poetic"
            }},
            {"B-verb", new List<string>{
                "surfing", "raining", "swimming", "diving", "dancing", "drowning", "bubbling", "crashing", "chilling", "glimmering"
            }},
            {"B-thing", new List<string>{
                "water", "ice", "wave", "illusion", "mist", "rain", "charm", "bubble", "snow", "hail", "wine", "river", "sight", "blizzard", "trick", "dance", "mind", "ice", "transmutation", "calculation", "counter", "tale", "fable", "lagoon", "tail", "fin", "storm", "icicle", "pursuit", "climate",  "frost", "riddle", "rhyme", "jewel", "potion", "ward", "poem"
            }},
            {"B-being", new List<string>{
                "leviathan", "illusionist", "sorcerer", "mushishi", "mystic", "siren", "traveler", "bard", "serpent", "frog", "shark"
            }},
            {"B-of", new List<string>{
                "of Ice", "of Frost", "of the Deep", "Riddle", "Illusion", "of the Mind"
            }},
            {"Y-adjective", new List<string>{
                "golden", "yellow", "barbaric", "stoic", "clay", "stone", "iron", "steel", "slate", "sandy",
            }},
            {"Y-verb", new List<string>{
                "pounding", "blocking", "stampeding", "ancient", "binding", "erupting", "imploding", "smashing"
            }},
            {"Y-thing", new List<string>{
                "earth", "rock", "stone", "steel", "upheaval", "quake", "tremor", "fissure", "iron", "sand", "mud", "punch", "crush", "desert", "tomb", "skin", "body", "statue", "defense", "fist", "shield", "mineral", "shard", "hammer", "axe", "grasp", "gate", "resist", "strength", "boulder", "protection", "guard", "clay", "planet", "colossal", "earth Dragon's", "slate"
            }},
            {"Y-being", new List<string>{
                "giant", "ancient", "titan", "golem", "earth Dragon", "gambler",
            }},
            {"Y-of", new List<string>{
                "of the Sand", "of the Land", "of the Earth", "of Iron", "of Power", "of Strength", "of Fortune"
            }},
            {"G-adjective", new List<string>{
                "green", "wooden", "leafy", "lush", "organic", "feathery", "quick", "gnarled", "acidic", "herbal", "potent"
            }},
            {"G-verb", new List<string>{
                "rotting", "windey", "rejuvenating", "swarming", "striking", "wilting", "energizing"
            }},
            {"G-thing", new List<string>{
                "plant",  "wood", "roots", "rot", "poison", "growth", "wind", "leaf", "blade", "garden", "flower", "seed", "vine", "spring", "rejuvenate", "flora", "fauna", "feather", "terrain", "fortune", "wager", "strike", "arrow", "trunk", "swarm", "unguent", "potion", "attempt", "dodge",
                "agility", "branch", "plan", "strategy", "sprint", "energy", "fang", "claw", "acid", "herb"
            }},
            {"G-being", new List<string>{
                "nature", "druid", "ranger", "green Dragon"
            }},
            {"G-of", new List<string>{
                "of the Ages", "of the Forest", "of the Woods", "the Blade"
            }},

        };
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

        public Spell(string _recipe, string _color, string _name, int _totalCost, List<V2Import> _pattern, int _damage, string _spellEffect)
        {
            recipe = _recipe;
            color = _color;
            name = _name;
            totalCost = _totalCost;
            pattern = _pattern;
            damage = _damage;
            spellEffect = _spellEffect;
        }
    }

    [Serializable]
    public class V2Import
    {
        public int x;
        public int y;

        public V2Import(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}