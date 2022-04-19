using System.Collections.Generic;
using UnityEngine;

public class Green : Element
{
    public Green()
    {
        destroysOccupant = false;
        hasKnockback = true;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
            };
    }
}