using System.Collections.Generic;
using UnityEngine;

public class Black : Element
{
    public Black()
    {
        destroysOccupant = true;
        hasKnockback = false;
        spellSFX = UnityCore.Audio.AudioType.SFX_BLACK_HIT;
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