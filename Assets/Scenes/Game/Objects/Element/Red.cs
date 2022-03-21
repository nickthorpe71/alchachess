using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Red : Element
    {
        public Red()
        {
            spellDamage = 12;
            damagesEnemies = true;
            healsEnemies = false;
            damagesAllies = true;
            healsAllies = false;
            altersEnvironment = false;
            environment = new FireEnv();
            spellPattern = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(-1, 0),
                new Vector2(-2, 0)
            };
        }
    }
}