using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Logic;

public class Piece : MonoBehaviour
{
    // State
    public bool isDead { get; private set; }
    public bool isGold { get; private set; }
    public bool isBeingKnockedBack { get; set; }

    // Stats
    public int moveDistance { get; protected set; }
    public List<Vector2> movePattern { get; protected set; }

    // Graphics
    private GameObject _warpAnim;
    private GameObject _graphic;

    // Movement
    private bool _isMoving = false;
    private Vector3 _newPosition;
    private float _moveSpeed = 3;


    public void Init(bool isGold)
    {
        isDead = false;
        this.isGold = isGold;
        isBeingKnockedBack = false;

        _warpAnim = Resources.Load("Piece/Anims/WarpAnim") as GameObject;
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }

    void Update()
    {
        PieceMovementUpdate();
    }

    public void PieceMovementUpdate()
    {
        if (!_isMoving) return;

        float step = _moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _newPosition, step);

        if (transform.position == _newPosition)
            _isMoving = false;
    }

    public void Move(Vector2 startPos, Tile endTile, bool warp = true)
    {
        if (warp)
            StartCoroutine(WarpRoutine(startPos, endTile));
        else
        {
            isBeingKnockedBack = true;
            _newPosition = Helpers.V2toV3(endTile.pos, 0);
            _isMoving = true;
            StartCoroutine(CheckDestinationDestroy(endTile));
        }
    }

    IEnumerator WarpRoutine(Vector2 startPos, Tile endTile)
    {
        GameObject warpAnim = Instantiate(_warpAnim, transform.position, Quaternion.identity);
        Destroy(warpAnim, 2);
        yield return new WaitForSeconds(0.25f);
        _graphic.SetActive(false);

        gameObject.transform.position = new Vector3(endTile.pos.x, 0, endTile.pos.y);
        _graphic.SetActive(true);

        StartCoroutine(CheckDestinationDestroy(endTile));
    }

    IEnumerator CheckDestinationDestroy(Tile destination)
    {
        if (destination.activeEnvironment != null && destination.activeEnvironment.destroysOccupant)
        {
            yield return new WaitForSeconds(0.5f);
            // TODO: play destroy anim
            destination.KillPiece();
        }
    }

    public void KnockOffBoard(Vector2 direction)
    {
        Vector3 newPos = transform.position + Helpers.V2toV3(direction, 0.5f);
        StartCoroutine(KnockOffRoutine(newPos));
    }
    IEnumerator KnockOffRoutine(Vector3 newPos)
    {
        _newPosition = newPos;
        _isMoving = true;
        yield return new WaitForSeconds(0.3f);
        newPos.y = -6;
        _newPosition = newPos;
        _isMoving = true;
        yield return new WaitForSeconds(3f);
        Kill();
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
                        // if (!canTraverse)
                        //     toRemove.Add(index);
                    }
                    return inBounds && canTraverse;
                })
                .ToArray();

            // // remove intraversable directions from further consideration
            // activeDirections = activeDirections
            //     .Where((_, index) => !toRemove.Contains(index))
            //     .ToList();
            // patternWithStartAdjust = patternWithStartAdjust
            //     .Where((_, index) => !toRemove.Contains(index));

            possibleMoves.AddRange(validMoves);
        }

        return possibleMoves;
    }

    public void Kill()
    {
        isDead = true;
        transform.position = new Vector3(-2, 0, 3);
    }


}
