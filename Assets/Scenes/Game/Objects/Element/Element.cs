using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public bool destroysOccupant { get; protected set; }
    public bool hasKnockback { get; protected set; }
    public Environment environment { get; protected set; }
    public List<Vector2> spellPattern { get; protected set; }

}
