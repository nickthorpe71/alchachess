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

    public class ElementWords
    {
        public static Dictionary<string, List<string>> list = new Dictionary<string, List<string>>{
            {"D", new List<string>{
                "darkness", "hex", "curse", "shadow", "shade",
                "black", "thief", "traitor", "bone", "touch",
                "dimension", "gravity", "demi", "phase", "drain",
                "gloom", "fear", "flesh", "wraith", "blood",
                "phantom", "ghost", "haunting", "necro", "gruesome",
                "torture", "sinister", "stab", "choke", "bleeding",
                "gouge", "scream", "spectre", "ritual", "voodoo",
                "phase", "disguise", "demon", "gargoyle", "contract",
                "deal", "bite", "horror", "witch", "warlocks",
                "exorcist", "geist", "demonic", "moonlight", "moon",
                "hangman's", "banshee's", "abyss", "of the Pit", "grime",
                "putrid", "skull", "ghoul's"
                }
            },
            {"W", new List<string>{
                "light", "heal", "cure", "holy", "divine",
                "eden", "prayer", "serenity", "flash", "renew",
                "hymn", "sing", "spirit", "soul", "hope",
                "fade", "faith", "pure", "smite", "redemption",
                "inner", "psalm", "knowledge", "lore", "enigma", "white",
                "wings", "paradise", "beam", "plasma", "tradition",
                "photon", "blinding", "glaring", "beaming", "consuming",
                "pius", "angelic", "sailing", "soaring", "flying",
                "gliding", "breeze", "choice", "judgement", "protecting",
                "flash", "ray", "conviction", "game", "horn", "prison",
                "of Eden", "eden's", "of Light"
                }
            },
            {"R", new List<string>{
                "scorcher", "burning", "hell", "flame", "fireball",
                "fire", "combust", "lava", "fiery", "blast",
                "breath", "sun", "prism", "inferno", "hellish",
                "pyroblast", "flamestrike", "hellfire", "searing", "magma",
                "phoenix", "sizzle", "dragon", "burn", "immolate", "red",
                "flash", "bang", "boom", "magus", "wall",
                "link", "chasm", "choke", "scald", "fuel",
                "blazing", "detonate", "raging", "bull", "tiger",
                "lizard", "salamander", "cascade", "consuming", "burning",
                "spear", "attack", "roar", "chant", "meteor", "berserk",
                "red Dragon's", "of Flame", "of the Sun"
                }
            },
            {"B", new List<string>{
                "water", "ice", "glacial", "surf", "wave",
                "illusion", "mist", "rain", "aqua", "charm",
                "bubble", "snow", "hail", "wine", "river",
                "shapeless", "glass", "sight", "hydro", "blizzard",
                "dive", "swim", "trick", "dance", "mind", "blue",
                "slate", "ice", "glacial", "surf", "wave",
                "mystic", "transmutation", "calculation", "counter", "song",
                "tale", "fable", "lagoon", "tail", "fin",
                "storm", "icicle", "pursuit", "climate", "dip",
                "elegant", "frost", "riddle", "rhyme", "jewel", "lavishing",
                "siren's", "of Ice", "of Frost"
                }
            },
            {"Y", new List<string>{
                "earth", "gold", "yellow", "rock", "stone",
                "upheaval", "quake", "tremor", "fissure", "iron",
                "sand", "mud", "punch", "crush", "desert",
                "tomb", "skin", "body", "statue", "pound",
                "defense", "fist", "block", "shield", "stomp",
                "ground", "mineral", "shard", "hammer", "axe",
                "giant's", "ancient", "binding", "mold", "grasp",
                "wall", "gate", "erupting", "shape", "bones",
                "resist", "implosion", "strength", "barbaric", "boulder",
                "protection", "guard", "clay", "planet", "smash", "titan",
                "colossal", "earth Dragon's"
                }
            },
            {"G", new List<string>{
                "plant", "green", "wood", "roots", "rot",
                "poison", "growth", "wind", "leaf", "blade",
                "slice", "garden", "flower", "seed", "vines",
                "nature", "spring", "organic", "rejuvenate", "flora",
                "fauna", "mother's", "druid's", "ranger", "ages",
                "feather", "quick", "aero", "terrain", "fortune",
                "wager", "strike", "arrow", "trunk", "swarm",
                "snakes", "unguent", "potion", "attempt", "dodge",
                "agility", "branch", "plan", "strategy", "sprint",
                "rapid", "energy", "singing", "fang", "claw", "green Dragon's",
                "of the Forest", "herbal", "acid"}
            }
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