using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Yellow : Element
    {
        public Yellow()
        {
            spellDamage = 10;
            damagesEnemies = true;
            healsEnemies = false;
            damagesAllies = false;
            healsAllies = false;
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