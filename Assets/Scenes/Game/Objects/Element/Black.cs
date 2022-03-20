using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Black : Element
    {
        public Black()
        {
            spellDamage = 13;
            damagesEnemies = true;
            healsEnemies = false;
            damagesAllies = true;
            healsAllies = false;
            altersEnvironment = false;
            duration = 0;
            spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 0),
                new Vector2(-1, 0)
            };
        }
    }
}