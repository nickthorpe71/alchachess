using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Logic;

public class Piece : MonoBehaviour
{

    private bool isGold;
    private bool isDead;

    // Graphics
    private GameObject _warpAnim;
    private GameObject _graphic;

    // Movement
    public bool isBeingKnockedBack { get; set; }
    private bool _isMoving = false;
    private Vector3 _newPosition;
    private float _moveSpeed = 3;
    protected int moveDistance;
    protected List<Vector2> movePattern;

    public void Init(bool isGold)
    {
        isDead = false;
        this.isGold = isGold;
        isBeingKnockedBack = false;

        _warpAnim = Resources.Load("Piece/Anims/WarpAnim") as GameObject;
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }
    public PieceData GetData()
    {
        return new PieceData(
            moveDistance,
            movePattern,
            isGold,
            isDead
        );
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
            _newPosition = Helpers.V2toV3(endTile.GetPos(), 0);
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

        gameObject.transform.position = new Vector3(endTile.GetPos().x, 0, endTile.GetPos().y);
        _graphic.SetActive(true);

        StartCoroutine(CheckDestinationDestroy(endTile));
    }

    IEnumerator CheckDestinationDestroy(Tile destination)
    {
        if (destination.HasActiveEnvironment() && destination.EnvironmentDestroysOccupant())
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

    public List<Vector2> PossibleMoves(Board board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        IEnumerable<Vector2> patternWithStartAdjust = movePattern
            .Select(move => new Vector2(transform.position.x + move.x, transform.position.z + move.y));

        for (int layer = 0; layer < moveDistance; layer++)
        {
            // find all valid moves
            Vector2[] validMoves = patternWithStartAdjust
                .Select((dir, index) => new Vector2(
                    dir.x + layer * movePattern[index].x,
                    dir.y + layer * movePattern[index].y
                ))
                .Where((dir, index) =>
                {
                    bool inBounds = board.IsInBounds(dir);
                    bool canTraverse = false;
                    if (inBounds)
                        canTraverse = board.GetTile(dir).CanTraverse();
                    return inBounds && canTraverse;
                })
                .ToArray();

            possibleMoves.AddRange(validMoves);
        }

        return possibleMoves;
    }

    public void Kill()
    {
        isDead = true;
        transform.position = new Vector3(-2, 0, 3);
    }

    public bool IsGold() => isGold;

}
