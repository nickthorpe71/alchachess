using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Green : Element
    {
        public Green()
        {
            destroysOccupant = false;
            hasKnockback = true;
            environment = new PlantEnv();
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
        }
    }
}