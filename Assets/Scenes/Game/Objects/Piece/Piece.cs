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
    }

    public void Move(Vector2 startPos, Vector2 endPos)
    {

        StartCoroutine(MoveRoutine(startPos, endPos));
    }

    private IEnumerator MoveRoutine(Vector2 startPos, Vector2 endPos)
    {
        GameObject anim1 = Instantiate(_warpAnim, transform.position, Quaternion.identity);
        Destroy(anim1, 2);
        yield return new WaitForSeconds(0.25f);
        _graphic.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        gameObject.transform.position = new Vector3(endPos.x, 0, endPos.y);



        // GameObject anim2 = Instantiate(_warpAnim, transform.position, Quaternion.identity);
        // Destroy(anim2, 2);
        // yield return new WaitForSeconds(0.25f);
        _graphic.SetActive(true);
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
