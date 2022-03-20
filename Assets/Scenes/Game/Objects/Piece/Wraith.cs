using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Wraith : Piece
    {
        public Wraith(bool isGold) : base(isGold)
        {
            health = 55;
            maxHealth = 55;
            power = 12;
            moveDistance = 5;
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