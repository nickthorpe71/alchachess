using System.Collections.Generic;
using UnityEngine;

public class Shanks : Piece
{
    void Start()
    {
        moveDistance = 4;
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