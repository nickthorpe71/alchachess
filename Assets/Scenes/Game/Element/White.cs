using System.Collections.Generic;
using UnityEngine;

public class White : Element
{
    public White()
    {
        destroysOccupant = false;
        hasKnockback = true;
        spellSFX = UnityCore.Audio.AudioType.SFX_WHITE_HIT;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1)
            };
    }
}