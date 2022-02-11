using UnityEngine;
using System;
using System.Collections.Generic;

using Actions;
using Calc;
using Data;

public class GameLogic : MonoBehaviour
{
    // Data
    [SerializeField] public PlayerToken humanPlayer = PlayerToken.P1;
    [SerializeField] public PlayerToken aiPlayer = PlayerToken.P2;

    // GameState
    [NonSerialized] public Board board;
    [NonSerialized] public int turnCount = 1;
    [NonSerialized] public PlayerToken currentPlayer = PlayerToken.P1;
    private Tile currentHover = null;
    private Tile currentClicked = null;
    [HideInInspector] public bool humanCanInput;

    // References
    private Graphics graphics;
    private GameUI ui;

    // --- Lifecycle Methods ---
    private void Awake()
    {
        board = new Board();

        graphics = GetComponent<Graphics>();
        graphics.InstantiateInitialBoard(board);

        ui = GetComponent<GameUI>();

        // currentPlayer = PlayerC.RandomizeFirstTurn();
        humanCanInput = PlayerC.CanHumanInput(currentPlayer);
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
        if (newClickedTile.Contents == TileContents.Piece && newClickedTile.Piece.player == humanPlayer)
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
            if (!humanCanInput)
                return;

            Spell spell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles, humanPlayer, currentPlayer));

            // remove all state from all tiles
            board.tiles = BoardC.ChangeTilesState(
                board.tiles,
                new List<TileState> { TileState.isAOE, TileState.isClicked, TileState.isHighlighted, TileState.isHovered },
                false
            );

            ExecuteMove(currentClicked, currentHover, spell);
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

        graphics.ToggleAllPieceStatsUI(false);

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
            ui.spellView.Toggle(false);
            if (!graphics.pieceIsMoving)
                graphics.ShowPieceStats(new Vector2(currentHover.X, currentHover.Y), currentHover.Piece);
        }
        else // if hoverint an element
        {
            if (currentClicked == null) return;

            Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles, humanPlayer, currentPlayer));
            graphics.TogglePieceStatsUI(new Vector2(currentClicked.X, currentClicked.Y), false);

            // if piece is clicked and there are elements in our path
            if (currentClicked != null && potentialSpell != null)
            {
                // show stats of potential spell
                float colorMod = SpellC.ColorMod(currentClicked.Piece.element, "N", potentialSpell.color);
                ui.spellView.UpdateView(potentialSpell, currentClicked.Piece, colorMod);

                // show potential spell AOE
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isAOE },
                    true,
                    BoardC.CalculateAOEPatterns(potentialSpell.pattern, currentHover, currentClicked.Piece.player)
                );
            }
            else
            {
                ui.ToggleAllUI(false);
                graphics.ToggleAllPieceStatsUI(false);
            }
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void ExecuteMove(Tile start, Tile end, Spell spell)
    {
        // take control away from human player
        humanCanInput = false;
        currentClicked = null;
        currentHover = null;
        MovePhase(start, end, spell);
    }

    private void MovePhase(Tile start, Tile end, Spell spell)
    {
        // --- Data ---
        Tile startTile = TileC.Clone(start);
        Tile endTile = TileC.Clone(end);

        Vector2 startPos = new Vector2(startTile.X, startTile.Y);
        Vector2 endPos = new Vector2(endTile.X, endTile.Y);

        // update piece data and contents state of start and end tiles

        board.tiles = BoardC.MapTilesBetween(board.tiles, startPos, endPos, (tile, x, y) =>
            (new Vector2(x, y) == endPos)
            ? TileC.ReplacePiece(tile, startTile.Piece)
            : TileC.RemovePiece(tile)
        );

        // --- Graphics ---
        graphics.MovePieceGraphic(startPos, endPos, () => CastPhase(endTile, spell));
    }

    private void CastPhase(Tile end, Spell spell)
    {
        // --- Data --- 
        // if spell parameter is not null
        if (spell == null)
        {
            UpkeepPhase(end);
            return;
        }

        // save caster
        Tile caster = board.tiles[end.Y][end.X];

        // get aoe pattern
        List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, caster, caster.Piece.player);
        List<Vector2> nonPieceTilesInRange = new List<Vector2>();

        // apply damage to pieces in range
        Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(board.tiles, aoeRange);
        Dictionary<Vector2, Tile> targetsPostDmg = new Dictionary<Vector2, Tile>();

        // apply damage/healing to pieces
        foreach (KeyValuePair<Vector2, Tile> kvp in targetsPreDmg)
        {
            Piece piecePostSpell = PieceC.ApplySpellToPiece(caster.Piece, kvp.Value.Piece, spell);
            Tile tileWithNewPiece = TileC.ReplacePiece(board.tiles[(int)kvp.Key.y][(int)kvp.Key.x], piecePostSpell);
            board.tiles[(int)kvp.Key.y][(int)kvp.Key.x] = tileWithNewPiece;
            targetsPostDmg[kvp.Key] = tileWithNewPiece;
        };


        // if this spell alters the environment 
        if (SpellEffects.list[spell.color].AltersEnvironment)
        {
            // store tiles with no piece in range
            board.tiles = BoardC.MapTiles(board.tiles, tile =>
            {
                if (!BoardC.TileInRange(tile, aoeRange)) return tile;

                // and save positions to place environment pieces to send to graphics
                Tile tileCopy = TileC.Clone(tile);
                if (tileCopy.Contents != TileContents.Piece && tileCopy.Contents != TileContents.Environment)
                {
                    nonPieceTilesInRange.Add(new Vector2(tile.X, tile.Y));
                    tileCopy = TileC.UpdateRemainingEnvTime(tileCopy, SpellEffects.list[spell.color].Duration);
                    tileCopy = TileC.UpdateContents(tileCopy, TileContents.Environment);
                }
                return tileCopy;
            });
        }

        // --- Graphics ---
        // play spell animations
        graphics.PlayCastAnims(spell, nonPieceTilesInRange, (caster) => UpkeepPhase(caster), caster, targetsPreDmg, targetsPostDmg);
    }

    public void UpkeepPhase(Tile movedPiece)
    {
        // --- Data ---
        // scan board for dead pieces
        Dictionary<Vector2, Tile> deadTargets = new Dictionary<Vector2, Tile>();
        board.tiles = BoardC.MapTiles(board.tiles, tile =>
        {
            if (tile.Contents != TileContents.Piece || tile.Piece.health > 0) return tile;
            // if found remove from board data and create list to 
            //send to graphics to be removed as well
            deadTargets[new Vector2(tile.X, tile.Y)] = tile;
            return TileC.RemovePiece(tile);
        });

        // upkeep environment effects
        List<Vector2> environmentsToRemove = new List<Vector2>();
        board.tiles = BoardC.MapTiles(board.tiles, tile =>
        {
            if (tile.Contents != TileContents.Environment) return tile;
            // reduce count on environmet effects
            Tile tileCopy = TileC.UpdateRemainingEnvTime(tile, tile.RemainingTimeOnEnvironment - 1);

            // remove expired environmet effects from the board
            if (tileCopy.RemainingTimeOnEnvironment == 0)
            {
                environmentsToRemove.Add(new Vector2(tileCopy.X, tileCopy.Y));
                tileCopy = TileC.UpdateContents(tileCopy, TileContents.Empty);
            }

            return tileCopy;
        });

        // restore all elements to the field
        Dictionary<Vector2, string> toRepopulate = new Dictionary<Vector2, string>();
        board.tiles = BoardC.RepopulateElements(board.tiles, toRepopulate);

        // --- Graphics ---
        // show effect animations and remove health and destroy newly dead targets
        graphics.PlayUpkeepAnims(NextTurnPhase, movedPiece, deadTargets, toRepopulate, environmentsToRemove);
    }

    public void NextTurnPhase()
    {
        // increment turn counter
        turnCount++;

        // switch current player token
        currentPlayer = PlayerC.GetOppositePlayer(currentPlayer);

        // give control to the correct player
        humanCanInput = PlayerC.CanHumanInput(currentPlayer);

        // wait for input
        if (currentPlayer != humanPlayer)
            AI.TakeTurn(board, this);
    }
}

// 9h per week
// TODO:
// - collecting opposite elements removes them
// - opponent should have a move anim where they wave their hand which will be played before their piece moves
// - opponent should have a talk animation and should have voice lines happen randomly after some events
// - pieces landing on opposite color spell aren't being penalized
// - improve AI (4h)
// - implement minimax (4h)
// --- 2/6/2022 ---
// --- Week away so only 5 hours  ---
// - improve VFX (3h)
// - add sound (2h)
// --- 2/13/2022 ---
// - improve UI (make sure player easily knows what's going on) (3h)
// - create mechanic to walk around then sit at table (3h)
// - write simple story (3h)
// --- 2/20/2022 ---
// - create environment for story (4h)
// - do multiplayer course (5h)
// --- 2/27/2022 ---
// - add multiplayer (4h)
// - polish (5h)
// --- 3/5/2022 ---
// - deploy (9h)
// --- 3/12/2022 ---