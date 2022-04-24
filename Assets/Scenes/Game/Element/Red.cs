using System.Collections.Generic;
using UnityEngine;

public class Red : Element
{
    public Red()
    {
        destroysOccupant = true;
        hasKnockback = false;
        spellSFX = UnityCore.Audio.AudioType.SFX_RED_HIT;
        spellPattern = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
    }
}