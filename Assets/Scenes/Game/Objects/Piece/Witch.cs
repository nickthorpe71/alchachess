using System.Collections.Generic;
using UnityEngine;

public class Witch : Piece
{
    void Start()
    {
        moveDistance = 3;
        movePattern = MovementPattern();
    }

    private List<Vector2> MovementPattern()
    {
        return new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1)
            };
    }
}