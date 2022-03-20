using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Objects;

namespace Logic
{
    public static class PieceLogic
    {
        public static List<Vector2> PossibleMoves(Piece piece, Board board, Tile start, Tile end)
        {
            List<Vector2> possibleMoves = new List<Vector2>();
            int startX = start.x;
            int startY = start.y;
            List<Vector2> activeDirections = piece.movePattern;

            for (int layer = 1; layer <= piece.moveDistance; layer++)
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
                for (int i = 0; i < piece.movePattern.Count; i++)
                    if (!board.GetTile(piece.movePattern[i]).CanTraverse())
                        activeDirections.Remove(piece.movePattern[i]);

                // filter directions to only include active directions
                directions = directions.Where(direction => activeDirections.Contains(direction)).ToList();

                possibleMoves.AddRange(directions);
            }
            return possibleMoves;
        }
    }
}

