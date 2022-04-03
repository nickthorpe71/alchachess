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
            spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1)
            };
        }

        public override void Cast()
        {
            Debug.Log($"cast {gameObject.name} element");
        }
    }
}