using System.Collections.Generic;
using UnityEngine;

public class PieceData
{
    public readonly int moveDistance;
    public readonly List<Vector2> movePattern;
    public readonly bool isGold;
    public readonly bool isDead;

    public PieceData(int moveDistance, List<Vector2> movePattern, bool isGold, bool isDead)
    {
        this.moveDistance = moveDistance;
        this.movePattern = movePattern;
        this.isGold = isGold;
        this.isDead = isDead;
    }
}
