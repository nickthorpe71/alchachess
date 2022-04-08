using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Logic;

public class Piece : MonoBehaviour
{
    public bool isDead { get; private set; }
    public bool isGold { get; private set; }
    public int moveDistance { get; protected set; }
    public List<Vector2> movePattern { get; protected set; }
    private GameObject _warpAnim;
    private GameObject _graphic;

    public void Init(bool isGold)
    {
        isDead = false;
        this.isGold = isGold;

        _warpAnim = Resources.Load("Piece/Anims/WarpAnim") as GameObject;
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }

    public void Kill()
    {
        isDead = true;
        transform.position = new Vector3(-2, 0, 3);
    }

    public void Move(Vector2 startPos, Vector2 endPos, bool warp = true)
    {
        if (warp)
            StartCoroutine(WarpRoutine(startPos, endPos));
        else
            Debug.Log("Lerp piece");
    }

    private IEnumerator WarpRoutine(Vector2 startPos, Vector2 endPos)
    {
        GameObject anim1 = Instantiate(_warpAnim, transform.position, Quaternion.identity);
        Destroy(anim1, 2);
        yield return new WaitForSeconds(0.25f);
        _graphic.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        gameObject.transform.position = new Vector3(endPos.x, 0, endPos.y);
        _graphic.SetActive(true);
    }

    public List<Vector2> PossibleMoves(Board board, Vector2 pos)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        List<Vector2> activeDirections = new List<Vector2>(movePattern);
        IEnumerable<Vector2> patternWithStartAdjust = movePattern
            .Select(move => new Vector2(pos.x + move.x, pos.y + move.y));

        for (int layer = 0; layer < moveDistance; layer++)
        {
            List<int> toRemove = new List<int>();

            // find all valid moves
            Vector2[] validMoves = patternWithStartAdjust
                .Select((dir, index) => new Vector2(
                    dir.x + layer * activeDirections[index].x,
                    dir.y + layer * activeDirections[index].y
                ))
                .Where((dir, index) =>
                {
                    bool inBounds = board.IsInBounds(dir);
                    bool canTraverse = false;
                    if (inBounds)
                    {
                        canTraverse = board.GetTile(dir).CanTraverse();
                        if (!canTraverse)
                            toRemove.Add(index);
                    }
                    return inBounds && canTraverse;
                })
                .ToArray();

            // remove intraversable directions from further consideration
            activeDirections = activeDirections
                .Where((_, index) => !toRemove.Contains(index))
                .ToList();
            patternWithStartAdjust = patternWithStartAdjust
                .Where((_, index) => !toRemove.Contains(index));

            possibleMoves.AddRange(validMoves);
        }

        return possibleMoves;
    }
}
