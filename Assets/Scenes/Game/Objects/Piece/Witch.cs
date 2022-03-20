using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Witch : Piece
    {
        public Witch(bool isGold) : base(isGold)
        {
            health = 20;
            maxHealth = 20;
            power = 8;
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
}