using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Data
    private GameData data;

    // State
    public bool localPlayerCanInput { get; private set; }

    // Systems
    private PlayerInput inputSystem;
    private Graphics graphics;

    // References
    public Board board { get; private set; }

    void Awake()
    {
        HumanPlayer p1 = new HumanPlayer(goldSide: true, isLocalPlayer: true);
        AIPlayer p2 = new AIPlayer(goldSide: false, isLocalPlayer: false);

        data = new GameData(p1, p2);
        data.SetStatus(GameStatus.ACTIVE);

        board = GetComponent<Board>();
        inputSystem = new PlayerInput(this);
        SetCanInput();
    }

    void Update()
    {
        inputSystem.HandleInput();
    }

    public GameObject Spawn(string objPath, Vector3 pos, Quaternion rot)
    {
        return Instantiate(Resources.Load(objPath) as GameObject, pos, rot);
    }

    public void SubmitMove(Vector2 start, Vector2 end)
    {
        Tile startTile = board.GetTile(start);
        Tile endTile = board.GetTile(end);

        bool validMove = startTile.GetPiece().PossibleMoves(board).Contains(end);
        if (!validMove) return;

        localPlayerCanInput = false;
        MoveData move = new MoveData(data.currentTurn, startTile.GetData(), endTile.GetData());
        data.AddPlayedMove(move);
        startTile.TransferPiece(to: endTile);
        board.SetAOEMarkers(endTile.GetPos(), deactivate: true);

        if (!endTile.HasActiveElement())
            NextTurn();
    }

    public bool IsEnd() => data.status != GameStatus.ACTIVE;

    public void NextTurn()
    {
        StartCoroutine(NextTurnRoutine());
    }
    IEnumerator NextTurnRoutine()
    {
        yield return new WaitForSeconds(1);
        board.RepopulateElements();
        yield return new WaitForSeconds(1);

        data.currentTurn = (data.currentTurn == data.players[0]) ? data.players[1] : data.players[0];
        SetCanInput();

        if (!data.currentTurn.isHumanPlayer)
            data.currentTurn.TakeTurn(this);

        // TODO: THIS SHOULD ONLY BE IF THERE ARE 2 LOCAL PLAYERS
        localPlayerCanInput = true;
    }

    private void SetCanInput()
    {
        localPlayerCanInput = data.currentTurn.isLocalPlayer;
    }

    public void SetStatus(GameStatus newStatus)
    {
        data.SetStatus(newStatus);
    }

    public GenericPlayer GetCurrentTurn() => data.currentTurn;
}
