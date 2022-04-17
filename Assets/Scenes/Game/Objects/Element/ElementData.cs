using System.Collections.Generic;
using UnityEngine;

public class ElementData
{
    private readonly bool destroysOccupant;
    private readonly bool hasKnockback;
    private readonly List<Vector2> spellPattern;
    private readonly string color;

    public ElementData(bool destroysOccupant, bool hasKnockback, List<Vector2> spellPattern, string color)
    {
        this.destroysOccupant = destroysOccupant;
        this.hasKnockback = hasKnockback;
        this.spellPattern = spellPattern;
        this.color = color;
    }
}
