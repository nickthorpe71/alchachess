using System.Collections.Generic;
using UnityEngine;

public class Blue : Element
{
    public Blue()
    {
        destroysOccupant = true;
        hasKnockback = false;
        spellSFX = UnityCore.Audio.AudioType.SFX_BLUE_HIT;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
            };
    }
}