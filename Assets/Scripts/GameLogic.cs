using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

using Actions;
using Calc;
using Data;

public class GameLogic : MonoBehaviour
{
    // Data
    [SerializeField] public PlayerData localPlayer;
    [SerializeField] public PlayerData remotePlayer;

    // GameState
    [HideInInspector] public Board board;
    [HideInInspector] public bool isPreGame;
    [HideInInspector] public int turnCount = 1;
    [HideInInspector] public PlayerToken currentPlayer;
    [HideInInspector] public PlayerToken oddPlayer;
    private Tile currentHover = null;
    private Tile currentClicked = null;
    [HideInInspector] public bool localPlayerCanInput;

    // References
    private Graphics graphics;
    private GameUI ui;

    // --- Lifecycle Methods ---
    private void Awake()
    {
        ui = GetComponent<GameUI>();
        graphics = GetComponent<Graphics>();
        graphics.Init(ui);
        board = new Board();
        isPreGame = true;

        // create players
        localPlayer = new PlayerData(PlayerToken.P1, PieceColor.White, false, false, new List<Piece>());
        remotePlayer = new PlayerData(PlayerToken.P2, PieceColor.Black, false, true, new List<Piece>());

        // currentPlayer = PlayerC.RandomizeFirstTurn();
        currentPlayer = localPlayer.PlayerToken;
        oddPlayer = localPlayer.PlayerToken;
        localPlayerCanInput = PlayerC.CanHumanInput(currentPlayer);
    }

    private void Start()
    {
        graphics.CollectTileGraphics();
    }

    void Update()
    {
        Player.HandleInput(this);
        graphics.PieceMovementUpdate();
    }

    // --- Logic ---
    public void TileClick(GameObject clicked)
    {
        // Get tile data for clicked tile
        Tile newClickedTile = BoardC.GetTile(board, new Vector2(clicked.transform.position.x, clicked.transform.position.z));

        // check if clicked tile has a piece owned by human player
        if (newClickedTile.Contents == TileContents.Piece)
        {
            ui.CloseCurrentPieceDetails();
            ui.TogglePieceUIPane(newClickedTile.Piece.Guid);
            // Reset temporary states
            board.tiles = BoardC.ChangeTilesState(board.tiles, new List<TileState> { TileState.isAOE, TileState.isHighlighted }, false);

            // if (newClickedTile.Piece.Player == localPlayer.PlayerToken)

            // if nothing currently clicked
            if (currentClicked == null)
            {
                // update isClicked state
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    true,
                    new List<Vector2> { new Vector2(newClickedTile.X, newClickedTile.Y) }
                );
                currentClicked = board.tiles[newClickedTile.Y][newClickedTile.X];

                // update highlight state
                if (newClickedTile.Piece.Player == localPlayer.PlayerToken)
                    board.tiles = BoardC.ChangeTilesState(
                        board.tiles,
                        new List<TileState> { TileState.isHighlighted },
                        true,
                        BoardC.PossibleMoves(board.tiles, currentClicked)
                    );
            }
            // if clicked the thing that is currently clicked
            else if (new Vector3(newClickedTile.X, 0, newClickedTile.Y) == new Vector3(currentClicked.X, 0, currentClicked.Y))
            {
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    false,
                    new List<Vector2> { new Vector2(currentClicked.X, currentClicked.Y) }
                );
                currentClicked = null;
            }
            // if we clicked a new piece
            else
            {
                // switch isClicked state on previously selected tile to false
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    false,
                    new List<Vector2> { new Vector2(currentClicked.X, currentClicked.Y) }
                );

                // switch previously selected tile to newly selected tile (which has isClicked state of true)
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    true,
                    new List<Vector2> { new Vector2(newClickedTile.X, newClickedTile.Y) }
                );
                currentClicked = board.tiles[newClickedTile.Y][newClickedTile.X];

                // update highlight data
                if (newClickedTile.Piece.Player == localPlayer.PlayerToken)
                    board.tiles = BoardC.ChangeTilesState(
                        board.tiles,
                        new List<TileState> { TileState.isHighlighted },
                        true,
                        BoardC.PossibleMoves(board.tiles, currentClicked)
                    );
            }
        }
        // if we clicked an element or empty tile which is highlighted
        else if (currentClicked != null && currentHover.IsHighlighted)
        {
            if (!localPlayerCanInput)
                return;

            // remove all state from all tiles
            board.tiles = BoardC.ChangeTilesState(
                board.tiles,
                new List<TileState> { TileState.isAOE, TileState.isClicked, TileState.isHighlighted, TileState.isHovered },
                false
            );

            ExecuteMove(currentClicked, currentHover);
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void TileHover(GameObject hovered)
    {
        if (hovered == null) return;

        Tile newHover = BoardC.GetTile(board, new Vector2(hovered.transform.position.x, hovered.transform.position.z));


        // if we are hovering on the same thing as before
        if (currentHover != null && new Vector3(newHover.X, 0, newHover.Y) == new Vector3(currentHover.X, 0, currentHover.Y))
            return;

        // on new hover remove all AOE markers
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isAOE },
            false
        );

        // if nothing has been hovered yet
        if (currentHover == null)
        {
            board.tiles = BoardC.ChangeTilesState(
                board.tiles,
                new List<TileState> { TileState.isHovered },
                true,
                new List<Vector2> { new Vector2(newHover.X, newHover.Y) }
            );
            currentHover = board.tiles[newHover.Y][newHover.X];
        }

        // if we are hovering on a new thing
        // Set old hovered to not hovered
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isHovered },
            false,
            new List<Vector2> { new Vector2(currentHover.X, currentHover.Y) }
        );

        // Set new Hovered to hovered TileState
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isHovered },
            true,
            new List<Vector2> { new Vector2(newHover.X, newHover.Y) }
        );
        currentHover = board.tiles[newHover.Y][newHover.X];

        // if hovering a piece
        if (currentHover.Contents == TileContents.Piece && localPlayerCanInput)
        {
            ui.TurnOffCurrentGlow();
            ui.ToggleSpellUI(false);
            ui.TogglePieceUIGlow(currentHover.Piece.Guid);
        }
        else // if hovering an element
        {
            ui.TurnOffCurrentGlow();

            // if piece is clicked and there are elements in our path
            if (currentClicked != null && currentHover.IsHighlighted)
            {
                Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(board, new Vector2(currentClicked.X, currentClicked.Y), new Vector2(currentHover.X, currentHover.Y)));
                if (potentialSpell != null)
                {
                    // show stats of potential spell
                    ui.UpdateSpellUI(potentialSpell, currentClicked.Piece);

                    // show potential spell AOE
                    board.tiles = BoardC.ChangeTilesState(
                        board.tiles,
                        new List<TileState> { TileState.isAOE },
                        true,
                        BoardC.CalculateAOEPatterns(potentialSpell.Pattern, currentHover, currentClicked.Piece.Player)
                    );
                }
            }
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void ExecuteMove(Tile start, Tile end)
    {
        // take control away from human player
        localPlayerCanInput = false;
        currentClicked = null;
        currentHover = null;
        // calc data
        MoveData moveData = BoardC.ExecuteMove(
            board,
            currentPlayer,
            new Vector2(start.X, start.Y),
            new Vector2(end.X, end.Y)
        );
        board = moveData.BoardPostMove;
        // send data and next phase to graphics for execution
        graphics.ExecuteMove(moveData, NextTurn);
    }

    public void NextTurn()
    {
        // switch current player token
        currentPlayer = PlayerC.GetOppositePlayer(currentPlayer);

        // give control to the correct player
        localPlayerCanInput = PlayerC.CanHumanInput(currentPlayer);

        turnCount++;

        // TODO: change delay back to 1f
        if (currentPlayer != localPlayer.PlayerToken)
            if (isPreGame) StartCoroutine(DelayActionWithLogic(0f, (gameLogic) => AI.ChoosePiece(gameLogic)));
            else AI.TakeTurn(board, this);
    }

    private void StartGame()
    {
        turnCount = 1;
        isPreGame = false;
        board = BoardC.RandomizeBoardElements(board);
        graphics.WipePieces();
        graphics.InstantiateInitialBoard(board);
        NextTurn();
    }

    public void SelectPiece(PieceLabel pieceLabel, PlayerToken player)
    {
        // generate a new piece
        Piece newPiece = PieceC.NewPieceFromTemplate(
            PieceTemplates.list[pieceLabel],
            PlayerC.GetPlayerDataByToken(
                player,
                localPlayer,
                remotePlayer
            )
        );

        // get current index and side of board
        int currentPieceIndex = player == oddPlayer ? (int)Mathf.Floor(turnCount / 2) : (turnCount / 2) - 1;
        int boardSide = player == localPlayer.PlayerToken ? 0 : 7;

        // add piece to row in UI (UI)
        ui.GetPieceUIsByPlayer(player)[currentPieceIndex].Init(newPiece);

        if (player == localPlayer.PlayerToken)
        {
            // add piece to player data (data)
            localPlayer = PlayerC.AddPiece(localPlayer, newPiece);
            // turn off UI for piece select
            ui.TogglePieceSelect(false);
        }
        else
        {
            // add piece to player data (data)
            remotePlayer = PlayerC.AddPiece(remotePlayer, newPiece);
            // turn on UI for piece select
            if (turnCount != Const.BOARD_WIDTH * 2)
                ui.TogglePieceSelect(true);
        }

        // spawn piece on board (graphics)
        graphics.InstantiatePiece(newPiece, currentPieceIndex, boardSide);

        // add to board data
        board.tiles[boardSide][currentPieceIndex] = TileC.UpdatePiece(board.tiles[boardSide][currentPieceIndex], newPiece);

        // TODO: change delay back to 2.5f
        // check if all pieces have been picked
        if (turnCount == Const.BOARD_WIDTH * 2)
            StartCoroutine(DelayAction(0f, StartGame));
        else // pass to other player to pick
            NextTurn();
    }

    // TODO: move to some generic tools monobehavior
    IEnumerator DelayActionWithLogic(float delay, Action<GameLogic> action)
    {
        yield return new WaitForSeconds(delay);
        action(this);
    }

    IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}