using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Red : Element
    {
        public Red()
        {
            destroysOccupant = true;
            hasKnockback = false;
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
        }

        public override void Cast()
        {
            Debug.Log($"cast {gameObject.name} element");
        }
    }
}