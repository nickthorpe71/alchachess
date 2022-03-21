using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Black : Element
    {
        public Black()
        {
            destroysOccupant = true;
            hasKnockback = false;
            environment = null;
            spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1)
            };
        }
    }
}