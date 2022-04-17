using System.Collections.Generic;
using UnityEngine;

public class PieceData
{
    private readonly int moveDistance;
    private readonly List<Vector2> movePattern;
    private readonly bool isGold;
    private readonly bool isDead;

    public PieceData(int moveDistance, List<Vector2> movePattern, bool isGold, bool isDead)
    {
        this.moveDistance = moveDistance;
        this.movePattern = movePattern;
        this.isGold = isGold;
        this.isDead = isDead;
    }
}
