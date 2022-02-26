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
        if (newClickedTile.Contents == TileContents.Piece && newClickedTile.Piece.Player == localPlayer.PlayerToken)
        {
            // Reset temporary states
            board.tiles = BoardC.ChangeTilesState(board.tiles, new List<TileState> { TileState.isAOE, TileState.isHighlighted }, false);

            // if nothing currently clicked
            if (currentClicked == null)
            {
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    true,
                    new List<Vector2> { new Vector2(newClickedTile.X, newClickedTile.Y) }
                );
                currentClicked = board.tiles[newClickedTile.Y][newClickedTile.X];

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
        if (currentHover.Contents == TileContents.Piece)
        {
            if (localPlayerCanInput)
                ui.ToggleSpellUI(false);
        }
        else // if hoverint an element
        {
            if (currentClicked == null) return;

            Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(board, new Vector2(currentClicked.X, currentClicked.Y), new Vector2(currentHover.X, currentHover.Y)));

            // if piece is clicked and there are elements in our path
            if (currentClicked != null && potentialSpell != null && currentHover.IsHighlighted)
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

    // private void MovePhase(Tile start, Tile end, Spell spell)
    // {
    //     // --- Data ---
    //     Tile startTile = TileC.Clone(start);
    //     Tile endTile = TileC.Clone(end);

    //     Vector2 startPos = new Vector2(startTile.X, startTile.Y);
    //     Vector2 endPos = new Vector2(endTile.X, endTile.Y);

    //     // update piece data and contents state of start and end tiles

    //     board.tiles = BoardC.MapTilesBetween(board.tiles, startPos, endPos, (tile, x, y) =>
    //         (new Vector2(x, y) == endPos)
    //         ? TileC.UpdatePiece(tile, startTile.Piece)
    //         : TileC.RemovePiece(tile)
    //     );

    //     // --- Graphics ---
    //     graphics.MovePieceGraphic(startPos, endPos, () => CastPhase(endTile, spell));
    // }

    // private void CastPhase(Tile end, Spell spell)
    // {
    //     // --- Data --- 
    //     // if spell parameter is not null
    //     if (spell == null)
    //     {
    //         UpkeepPhase(end);
    //         return;
    //     }

    //     // save caster
    //     Tile caster = board.tiles[end.Y][end.X];

    //     // get aoe pattern
    //     List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, caster, caster.Piece.player);
    //     List<Vector2> nonPieceTilesInRange = new List<Vector2>();

    //     // apply damage to pieces in range
    //     Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(board.tiles, aoeRange);
    //     Dictionary<Vector2, Tile> targetsPostDmg = new Dictionary<Vector2, Tile>();

    //     // apply damage/healing to pieces
    //     foreach (KeyValuePair<Vector2, Tile> kvp in targetsPreDmg)
    //     {
    //         Piece piecePostSpell = PieceC.ApplySpellToPiece(caster.Piece, kvp.Value.Piece, spell);
    //         Tile tileWithNewPiece = TileC.UpdatePiece(board.tiles[(int)kvp.Key.y][(int)kvp.Key.x], piecePostSpell);
    //         board.tiles[(int)kvp.Key.y][(int)kvp.Key.x] = tileWithNewPiece;
    //         targetsPostDmg[kvp.Key] = tileWithNewPiece;
    //     };


    //     // if this spell alters the environment 
    //     if (SpellEffects.list[spell.color].AltersEnvironment)
    //     {
    //         // store tiles with no piece in range
    //         board.tiles = BoardC.MapTiles(board.tiles, tile =>
    //         {
    //             if (!BoardC.TileInRange(tile, aoeRange)) return tile;

    //             // and save positions to place environment pieces to send to graphics
    //             Tile tileCopy = TileC.Clone(tile);
    //             if (tileCopy.Contents != TileContents.Piece && tileCopy.Contents != TileContents.Environment)
    //             {
    //                 nonPieceTilesInRange.Add(new Vector2(tile.X, tile.Y));
    //                 tileCopy = TileC.UpdateRemainingEnvTime(tileCopy, SpellEffects.list[spell.color].Duration);
    //                 tileCopy = TileC.UpdateContents(tileCopy, TileContents.Environment);
    //             }
    //             return tileCopy;
    //         });
    //     }

    //     // --- Graphics ---
    //     // play spell animations
    //     graphics.PlayCastAnims(spell, nonPieceTilesInRange, (caster) => UpkeepPhase(caster), caster, targetsPreDmg, targetsPostDmg);
    // }

    // public void UpkeepPhase(Tile movedPiece)
    // {
    //     // --- Data ---
    //     // scan board for dead pieces
    //     Dictionary<Vector2, Tile> deadTargets = new Dictionary<Vector2, Tile>();
    //     board.tiles = BoardC.MapTiles(board.tiles, tile =>
    //     {
    //         if (tile.Contents != TileContents.Piece || tile.Piece.health > 0) return tile;
    //         // if found remove from board data and create list to 
    //         //send to graphics to be removed as well
    //         deadTargets[new Vector2(tile.X, tile.Y)] = tile;
    //         return TileC.RemovePiece(tile);
    //     });

    //     // upkeep environment effects
    //     List<Vector2> environmentsToRemove = new List<Vector2>();
    //     board.tiles = BoardC.MapTiles(board.tiles, tile =>
    //     {
    //         if (tile.Contents != TileContents.Environment) return tile;
    //         // reduce count on environmet effects
    //         Tile tileCopy = TileC.UpdateRemainingEnvTime(tile, tile.RemainingTimeOnEnvironment - 1);

    //         // remove expired environmet effects from the board
    //         if (tileCopy.RemainingTimeOnEnvironment == 0)
    //         {
    //             environmentsToRemove.Add(new Vector2(tileCopy.X, tileCopy.Y));
    //             tileCopy = TileC.UpdateContents(tileCopy, TileContents.Empty);
    //         }

    //         return tileCopy;
    //     });

    //     // restore all elements to the field
    //     Dictionary<Vector2, string> toRepopulate = new Dictionary<Vector2, string>();
    //     board.tiles = BoardC.RepopulateElements(board.tiles, toRepopulate);

    //     // --- Graphics ---
    //     // show effect animations and remove health and destroy newly dead targets
    //     graphics.PlayUpkeepAnims(NextTurnPhase, movedPiece, deadTargets, toRepopulate, environmentsToRemove);
    // }

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
        ui.GetPieceUIByPlayer(player)[currentPieceIndex].Init(newPiece);

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