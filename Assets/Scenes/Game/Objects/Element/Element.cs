using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public abstract class Element
    {
        public List<Vector2> spellPattern { get; protected set; }
        public int spellDamage { get; protected set; }
        public bool hasKnockback { get; protected set; }
        public bool damagesEnemies { get; protected set; }
        public bool healsEnemies { get; protected set; }
        public bool damagesAllies { get; protected set; }
        public bool healsAllies { get; protected set; }
        public bool altersEnvironment { get; protected set; }
        public Environment environment { get; protected set; }
    }
}
