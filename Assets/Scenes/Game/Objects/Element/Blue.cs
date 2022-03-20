using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Blue : Element
    {
        public Blue()
        {
            spellDamage = 5;
            damagesEnemies = true;
            healsEnemies = false;
            damagesAllies = false;
            healsAllies = true;
            altersEnvironment = true;
            duration = 2;
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1)
            };
        }
    }
}