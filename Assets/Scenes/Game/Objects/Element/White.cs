using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class White : Element
    {
        public White()
        {
            destroysOccupant = false;
            hasKnockback = true;
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(-1, 0)
            };
        }

        private void Awake()
        {
            SetDestroyAnim("White");
        }


        public override void Cast()
        {
            Debug.Log($"cast {gameObject.name} element");
        }
    }
}