using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Data
    public GameData gameData { get; private set; }

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

        gameData = new GameData(p1, p2);
        gameData.SetStatus(GameStatus.ACTIVE);

        board = GetComponent<Board>();
        inputSystem = new PlayerInput(this);
        SetCanInput();
    }

    void Update()
    {
        inputSystem.HandleInput();
    }

    public void SubmitMove(Vector2 start, Vector2 end)
    {
        Tile startTile = BoardCalculation.GetTile(board.boardData, start);
        Tile endTile = BoardCalculation.GetTile(board.boardData, end);

        bool validMove = startTile.GetPiece().PossibleMoves(board.boardData, start).Contains(end);
        if (!validMove) return;

        localPlayerCanInput = false;
        MoveData move = new MoveData(gameData.currentTurn, startTile, endTile);
        gameData.AddPlayedMove(move);
        startTile.TransferPiece(to: endTile);
        board.SetAOEMarkers(endTile.pos, deactivate: true);

        if (!endTile.HasActiveElement())
            NextTurn();
    }

    public bool IsEnd() => gameData.status != GameStatus.ACTIVE;

    public void NextTurn()
    {
        StartCoroutine(NextTurnRoutine());
    }
    IEnumerator NextTurnRoutine()
    {
        yield return new WaitForSeconds(1);
        BoardCalculation.RepopulateElements(board.boardData);
        yield return new WaitForSeconds(1);

        gameData.currentTurn = (gameData.currentTurn == gameData.players[0]) ? gameData.players[1] : gameData.players[0];
        SetCanInput();

        if (!gameData.currentTurn.isHumanPlayer)
            gameData.currentTurn.TakeTurn(this);

        // TODO: THIS SHOULD ONLY BE IF THERE ARE 2 LOCAL PLAYERS
        localPlayerCanInput = true;
    }

    private void SetCanInput()
    {
        localPlayerCanInput = gameData.currentTurn.isLocalPlayer;
    }

    public void SetStatus(GameStatus newStatus)
    {
        gameData.SetStatus(newStatus);
    }
}
