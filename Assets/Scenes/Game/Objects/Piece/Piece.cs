using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public abstract class Piece
    {
        public bool isDead { get; private set; }
        private readonly bool _isGold;
        public bool isGold { get { return _isGold; } }
        private readonly Guid _guid;
        public Guid guid { get { return _guid; } }
        public float health { get; protected set; }
        protected float maxHealth;
        protected float power;
        public int moveDistance { get; protected set; }
        public List<Vector2> movePattern { get; protected set; }

        public Piece(bool isGold)
        {
            isDead = false;
            _isGold = isGold;
            _guid = Guid.NewGuid();
        }

        public void Damage(int amount)
        {
            health = Mathf.Max(health - amount, 0);
        }

        public void Heal(int amount)
        {
            health = Mathf.Min(health + amount, maxHealth);
        }

        public List<Vector2> PossibleMoves(Board board, Tile start, Tile end)
        {
            List<Vector2> possibleMoves = new List<Vector2>();
            int startX = start.x;
            int startY = start.y;
            List<Vector2> activeDirections = movePattern;

            for (int layer = 1; layer <= moveDistance; layer++)
            {
                List<Vector2> directions = new List<Vector2>(){
                    new Vector2(startX, startY + layer),
                    new Vector2(startX + layer, startY + layer),
                    new Vector2(startX + layer, startY),
                    new Vector2(startX + layer, startY - layer),
                    new Vector2(startX, startY - layer),
                    new Vector2(startX - layer, startY - layer),
                    new Vector2(startX - layer, startY),
                    new Vector2(startX - layer, startY + layer)
                };

                // remove any active directions that are not traversable
                for (int i = 0; i < movePattern.Count; i++)
                    if (!board.GetTile(movePattern[i]).CanTraverse())
                        activeDirections.Remove(movePattern[i]);

                // filter directions to only include active directions
                directions = directions.Where(direction => activeDirections.Contains(direction)).ToList();

                possibleMoves.AddRange(directions);
            }
            return possibleMoves;
        }
    }
}
