using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isDead { get; private set; }
    private bool isGold;
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
        int startX = (int)pos.x;
        int startY = (int)pos.y;
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
                if (!board.GetTile(movePattern[i]).GetComponent<Tile>().CanTraverse())
                    activeDirections.Remove(movePattern[i]);

            // filter directions to only include active directions
            directions = directions.Where(direction => activeDirections.Contains(direction)).ToList();

            possibleMoves.AddRange(directions);
        }
        return possibleMoves;
    }
}
