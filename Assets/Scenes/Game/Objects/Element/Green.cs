using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Green : Element
    {
        public Green()
        {
            spellDamage = 8;
            damagesEnemies = true;
            healsEnemies = false;
            damagesAllies = false;
            healsAllies = true;
            altersEnvironment = true;
            environment = new PlantEnv();
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(-1, -1),
                new Vector2(1, -1)
            };
        }
    }
}