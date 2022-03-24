using System.Collections.Generic;
using UnityEngine;

public class GodOfLife : Piece
{
    void Start()
    {
        moveDistance = 1;
        movePattern = MovementPattern();
    }

    private List<Vector2> MovementPattern()
    {
        return new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1)
            };
    }
}