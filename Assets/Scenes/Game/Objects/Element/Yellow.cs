using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Yellow : Element
    {
        public Yellow()
        {
            destroysOccupant = false;
            hasKnockback = true;
            spellPattern = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(-1, 0)
            };
        }

        private void Awake()
        {
            SetDestroyAnim("Yellow");
        }


        public override void Cast()
        {
            Debug.Log($"cast {gameObject.name} element");
        }
    }
}