using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class White : Element
    {
        public White()
        {
            spellDamage = 12;
            damagesEnemies = false;
            healsEnemies = true;
            damagesAllies = false;
            healsAllies = true;
            altersEnvironment = false;
            duration = 0;
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