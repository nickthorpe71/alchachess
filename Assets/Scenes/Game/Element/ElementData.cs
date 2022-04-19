using System.Collections.Generic;
using UnityEngine;

public class ElementData
{
    public readonly bool destroysOccupant;
    public readonly bool hasKnockback;
    public readonly List<Vector2> spellPattern;
    public readonly string color;

    public ElementData(bool destroysOccupant, bool hasKnockback, List<Vector2> spellPattern, string color)
    {
        this.destroysOccupant = destroysOccupant;
        this.hasKnockback = hasKnockback;
        this.spellPattern = spellPattern;
        this.color = color;
    }
}
