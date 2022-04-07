using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Blue : Element
    {
        public Blue()
        {
            destroysOccupant = true;
            hasKnockback = false;
            spellPattern = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(-1, 0)
            };
        }
    }
}