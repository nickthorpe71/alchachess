using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public abstract class Element
    {
        public bool destroysOccupant { get; protected set; }
        public bool hasKnockback { get; protected set; }
        public Environment environment { get; protected set; }
        public List<Vector2> spellPattern { get; protected set; }

    }
}
