using System.Collections.Generic;
using UnityEngine;

namespace Data
{

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
            {"D", new ElementalComponent("D", new List<Vector2>{new Vector2(1, -1), new Vector2(-1, -1)}, 2)},
            {"W", new ElementalComponent("W", new List<Vector2>{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)}, 2)},
            {"R", new ElementalComponent("R", new List<Vector2>{new Vector2(0, 1),new Vector2(0, 2)}, 1)},
            {"B", new ElementalComponent("B", new List<Vector2>{new Vector2(1, 0), new Vector2(-1, 0)}, 1)},
            {"Y", new ElementalComponent("Y", new List<Vector2>{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1)}, 1)},
            {"G", new ElementalComponent("G", new List<Vector2>{new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)}, 1)}
        };
    }

    public class ElementalComponent
    {
        private readonly string _element;
        private readonly List<Vector2> _pattern;
        private readonly int _damage;

        public ElementalComponent(string element, List<Vector2> pattern, int damage)
        {
            _element = element;
            _pattern = pattern;
            _damage = damage;
        }

        public string Element
        {
            get { return _element; }
        }

        public List<Vector2> Pattern
        {
            get { return _pattern; }
        }

        public int Damage
        {
            get { return _damage; }
        }
    }

    public static class NameLibrary
    {
        public static Dictionary<string, string> list = new Dictionary<string, string>();
    }

    public static class NamePatterns
    {
        public static Dictionary<int, List<List<string>>> list = new Dictionary<int, List<List<string>>>
        {
            {2, new List<List<string>>{
                new List<string>{"adjective", "being"},
                new List<string>{"adjective", "thing"},
                new List<string>{"verb", "being"},
                new List<string>{"verb", "thing"},
                new List<string>{"adjective", "being"},
                new List<string>{"adjective", "being"},
                new List<string>{"being's", "thing"},
                new List<string>{"thing", "being"}
            }},
            {3, new List<List<string>>{
                new List<string>{"adjective", "being", "of"},
                new List<string>{"adjective", "thing", "of"},
                new List<string>{"adjective", "adjective", "being"},
                new List<string>{"adjective", "verb", "being"},
                new List<string>{"verb", "being", "thing"},
                new List<string>{"verb", "being", "of"},
                new List<string>{"verb", "adjective", "being"},
                new List<string>{"being's", "adjective", "thing"},
                new List<string>{"being's", "thing", "of"},
                new List<string>{"being's", "adjective", "being"},
                new List<string>{"thing", "being's", "thing"}
            }},
            {4, new List<List<string>>{
                new List<string>{"adjective", "adjective", "being", "of"},
                new List<string>{"adjective", "being's", "verb", "being"},
                new List<string>{"adjective", "being's", "verb", "thing"},
                new List<string>{"adjective", "verb", "being's", "thing"},
                new List<string>{"adjective", "verb", "being", "of"},
                new List<string>{"verb", "being", "thing", "of"},
                new List<string>{"verb", "being's", "thing", "of"},
                new List<string>{"verb", "adjective", "being", "thing"},
                new List<string>{"verb", "adjective", "being", "of"},
                new List<string>{"being's", "adjective", "thing", "of"},
                new List<string>{"being's", "adjective", "adjective", "being"},
                new List<string>{"being's", "adjective", "adjective", "thing"},
                new List<string>{"being's", "adjective", "being", "of"},
                new List<string>{"thing", "being's", "thing", "of"},
                new List<string>{"thing", "being's", "adjective", "thing"}
            }},
            {5, new List<List<string>>{
                new List<string>{"adjective", "being's", "verb", "adjective", "being"},
                new List<string>{"adjective", "being's", "verb", "being", "of"},
                new List<string>{"adjective", "being's", "verb", "thing", "of"},
                new List<string>{"adjective", "verb", "being's", "thing", "of"},
                new List<string>{"verb", "adjective", "adjective", "being", "thing"},
                new List<string>{"verb", "adjective", "being", "thing", "of"},
                new List<string>{"being's", "adjective", "adjective", "being", "of"},
                new List<string>{"being's", "adjective", "adjective", "thing", "of"},
                new List<string>{"being's", "adjective", "adjective", "verb", "thing"},
                new List<string>{"thing", "being's", "adjective", "thing", "of"},
            }}
        };
    }

    public static class ElementWords
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
                "of Flame", "of the Sun", "of Hellfire", "of the Tiger", "of the Lizard", "of the Berserker", "of the Dragon", "of the Salamander", "of the Bull", "of Lightning"
            }},
            {"B-adjective", new List<string>{
                "glacial", "aqua", "charming", "wet", "cold", "frozen", "shapeless", "glass", "blue", "mystical", "legendary", "forgotten", "invisible", "elegant", "lavishing", "romantic", "drunken", "poetic"
            }},
            {"B-verb", new List<string>{
                "surfing", "swimming", "diving", "dancing", "drowning", "bubbling", "crashing", "chilling", "glimmering"
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

    public static class SpellEffects
    {
        public static Dictionary<string, SpellEffect> list = new Dictionary<string, SpellEffect>{
            {"D", new SpellEffect(false, true, false, true, false, 0)},
            {"W", new SpellEffect(false, false, true, false, true, 0)},
            {"R", new SpellEffect(false, true, false, false, false, 0)},
            {"B", new SpellEffect(false, true, false, false, true, 0)},
            {"Y", new SpellEffect(true, true, false, false, false, 2)},
            {"G", new SpellEffect(true, false, false, false, true, 2)},
        };
    }

    public class SpellEffect
    {
        private readonly bool _altersEnvironment;
        private readonly bool _damagesEnemies;
        private readonly bool _healsEnemies;
        private readonly bool _damagesAllies;
        private readonly bool _healsAllies;
        private readonly int _duration;

        public SpellEffect(
            bool altersEnvironment,
            bool damagesEnemies,
            bool healsEnemies,
            bool damagesAllies,
            bool healsAllies,
            int duration
            )
        {
            _altersEnvironment = altersEnvironment;
            _damagesEnemies = damagesEnemies;
            _healsEnemies = healsEnemies;
            _damagesAllies = damagesAllies;
            _healsAllies = healsAllies;
            _duration = duration;
        }

        public bool AltersEnvironment { get { return _altersEnvironment; } }
        public bool DamagesEnemies { get { return _damagesEnemies; } }
        public bool HealsEnemies { get { return _healsEnemies; } }
        public bool DamagesAllies { get { return _damagesAllies; } }
        public bool HealsAllies { get { return _healsAllies; } }
        public int Duration { get { return _duration; } }
    }

    public class Spell
    {
        private readonly string _recipe;
        private readonly string _color;
        private readonly string _name;
        private readonly int _totalCost;
        private readonly List<Vector2> _pattern;
        private readonly int _damage;

        public Spell(string recipe, string color, string name, int totalCost, List<Vector2> pattern, int damage)
        {
            _recipe = recipe;
            _color = color;
            _name = name;
            _totalCost = totalCost;
            _pattern = pattern;
            _damage = damage;
        }

        public string Recipe { get { return _recipe; } }
        public string Color { get { return _color; } }
        public string Name { get { return _name; } }
        public int TotalCost { get { return _totalCost; } }
        public List<Vector2> Pattern { get { return _pattern; } }
        public int Damage { get { return _damage; } }
    }
}