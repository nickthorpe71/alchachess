using System.Collections.Generic;
using UnityEngine;

public class Green : Element
{
    public Green()
    {
        destroysOccupant = false;
        hasKnockback = true;
        spellSFX = UnityCore.Audio.AudioType.SFX_GREEN_HIT;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
            };
    }
}