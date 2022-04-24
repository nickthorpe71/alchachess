using System.Collections.Generic;
using UnityEngine;

public class Yellow : Element
{
    public Yellow()
    {
        destroysOccupant = false;
        hasKnockback = true;
        spellSFX = UnityCore.Audio.AudioType.SFX_YELLOW_HIT;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
    }
}