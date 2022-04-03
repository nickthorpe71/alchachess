using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isDead { get; private set; }
    public bool isGold { get; private set; }
    public int moveDistance { get; protected set; }
    public List<Vector2> movePattern { get; protected set; }

    public void Init(bool isGold)
    {
        isDead = false;
        this.isGold = isGold;
    }

    public void Kill()
    {
        isDead = true;
    }

    public List<Vector2> PossibleMoves(Board board, Vector2 pos)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        Vector2[] activeDirections = movePattern
            .Select(move => new Vector2(pos.x + move.x, pos.y + move.y))
            .ToArray();

        for (int layer = 1; layer <= moveDistance; layer++)
        {
            Vector2[] validMoves = activeDirections
                .Select(dir => new Vector2(layer * dir.x, layer * dir.y))
                .Where(move => board.IsInBounds(move) && board.GetTile(move).CanTraverse())
                .ToArray();

            possibleMoves.AddRange(validMoves);
        }
        return possibleMoves;
    }
}
