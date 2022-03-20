using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class GodOfLife : Piece
    {
        public GodOfLife(bool isGold) : base(isGold)
        {
            health = 50;
            maxHealth = 50;
            power = 10;
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
}