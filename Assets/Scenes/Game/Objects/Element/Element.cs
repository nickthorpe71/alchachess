using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public bool destroysOccupant { get; protected set; }
    public bool hasKnockback { get; protected set; }
    public GameObject environmentPrefab;
    public List<Vector2> spellPattern { get; protected set; }

    public abstract void Cast();

    public List<Vector2> GetSpellPattern(Board board, Vector2 pos)
    {
        return spellPattern
            .Select(target => new Vector2(target.x + pos.x, target.y + pos.y))
            .Where(target => board.IsInBounds(target)).ToList();
    }
}
