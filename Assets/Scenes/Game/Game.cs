using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Data
    private GenericPlayer[] players;
    public GenericPlayer currentTurn { get; private set; }
    public GameStatus status { get; private set; }
    private List<MoveData> _movesPlayed;

    // State
    public bool localPlayerCanInput { get; private set; }

    // Systems
    private PlayerInput inputSystem;
    private GameUI ui;

    // References
    public Board board { get; private set; }

    void Awake()
    {
        ui = GetComponent<GameUI>();

        HumanPlayer p1 = new HumanPlayer(goldSide: true, isLocalPlayer: true);
        AIPlayer p2 = new AIPlayer(goldSide: false, isLocalPlayer: false);

        players = new GenericPlayer[2];
        players[0] = p1;
        players[1] = p2;

        SetStatus(GameStatus.ACTIVE);

        currentTurn = p1;

        _movesPlayed = new List<MoveData>();

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
        MoveData move = new MoveData(currentTurn, startTile.GetData(), endTile.GetData());
        _movesPlayed.Add(move);
        startTile.TransferPiece(to: endTile);
        board.SetAOEMarkers(endTile.GetPos(), deactivate: true);

        if (!endTile.HasActiveElement())
            NextTurn();
    }

    public bool IsEnd() => status != GameStatus.ACTIVE;

    public void NextTurn()
    {
        if (IsGameOver())
            StartCoroutine(GameOverRoutine(currentTurn.isLocalPlayer));
        else
            StartCoroutine(NextTurnRoutine());
    }

    private bool IsGameOver()
    {
        int opponentPieceCount = board.GetTilePiecesForPlayer(GetOppositePlayer()).Count;
        return opponentPieceCount == 0;
    }

    IEnumerator GameOverRoutine(bool localPlayerWon)
    {
        yield return new WaitForSeconds(3);

        if (localPlayerWon)
            ui.DisplayWin();
        else
            ui.DisplayLose();

        yield return new WaitForSeconds(5);

        GlobalSceneLoader.LoadScene("MainMenu");
    }

    IEnumerator NextTurnRoutine()
    {
        yield return new WaitForSeconds(2.6f);
        board.RepopulateElements();

        currentTurn = GetOppositePlayer();
        SetCanInput();

        if (!currentTurn.isHumanPlayer)
        {
            yield return new WaitForSeconds(2);
            currentTurn.TakeTurn(this);
        }

        // // TODO: THIS SHOULD ONLY BE IF THERE ARE 2 LOCAL PLAYERS
        // localPlayerCanInput = true;
    }

    private void SetCanInput()
    {
        localPlayerCanInput = currentTurn.isLocalPlayer;
    }
    private GenericPlayer GetOppositePlayer()
    {
        return (currentTurn == players[0]) ? players[1] : players[0];
    }

    public void SetStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public GenericPlayer GetCurrentTurn() => currentTurn;
}
