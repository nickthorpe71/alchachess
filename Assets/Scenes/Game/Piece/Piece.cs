using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // General
    private bool isGold;
    private bool isDead;
    private BoxCollider _hitBox;

    // Graphics
    private GameObject _warpAnim;
    private GameObject _graphic;
    [SerializeField] private GameObject blueModel;
    [SerializeField] private GameObject redModel;
    private Animator _animator;

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

        _warpAnim = Resources.Load("Piece/Anims/Shared/WarpAnim") as GameObject;

        _graphic = isGold ? blueModel : redModel;
        blueModel.SetActive(isGold);
        redModel.SetActive(!isGold);

        _animator = _graphic.GetComponent<Animator>();

        _hitBox = GetComponent<BoxCollider>();
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
        if (!_isMoving)
        {
            if (isBeingKnockedBack)
                SetKnockback(false);
            return;
        }

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
        _animator.SetTrigger("warp");
        Destroy(warpAnim, 2);
        yield return new WaitForSeconds(0.45f);
        _graphic.SetActive(false);

        gameObject.transform.position = new Vector3(endTile.GetPos().x, 0, endTile.GetPos().y);
        _graphic.SetActive(true);
        _animator.SetTrigger("endWarp");

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

        _animator.SetBool("isDead", true);
        _newPosition = newPos;
        _isMoving = true;
        yield return new WaitForSeconds(0.3f);
        newPos.y = -6;
        _newPosition = newPos;
        _isMoving = true;
        yield return new WaitForSeconds(3f);
        Kill();
    }

    public void SetKnockback(bool isKnockedBack)
    {
        _animator.SetBool("knockedBack", isKnockedBack);
    }

    public void StartCastAnim(string spellColor)
    {
        string triggerName = "";

        switch (spellColor)
        {
            case "Red":
                triggerName = "summon";
                break;
            case "Blue":
                triggerName = "summon";
                break;
            case "Green":
                triggerName = "radialCast";
                break;
            case "Yellow":
                triggerName = "radialCast";
                break;
            case "Black":
                triggerName = "summon";
                break;
            case "White":
                triggerName = "radialCast";
                break;
        }

        _animator.SetTrigger(triggerName);
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
        _animator.SetBool("isDead", true);
        isDead = true;
        _hitBox.enabled = false;
        StartCoroutine(KillRoutine());
    }
    IEnumerator KillRoutine()
    {
        yield return new WaitForSeconds(4);
        transform.position = new Vector3(-12, 0, 3);
    }

    public bool IsGold() => isGold;
}
