using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public bool destroysOccupant { get; protected set; }
    public bool hasKnockback { get; protected set; }
    public GameObject environmentPrefab;
    public List<Vector2> spellPattern { get; protected set; }

    public abstract void Cast();
}
