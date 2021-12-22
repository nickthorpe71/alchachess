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
    public TextAsset masterSpellList;

    // GameState
    [NonSerialized] public Board board;
    [NonSerialized] public int turnCount = 0;
    [NonSerialized] public PlayerToken currentTurn = PlayerToken.P1;
    [NonSerialized] public bool pieceClicked = false;
    private Tile currentHover = null;
    private Tile currentClicked = null;

    // References
    private Graphics graphics;
    private GameUI ui;

    // --- Lifecycle Methods ---
    private void Awake()
    {
        board = new Board();

        SpellLoader.LoadAllSpells(masterSpellList);

        graphics = GetComponent<Graphics>();
        graphics.InstantiateInitialBoard(board);

        ui = GetComponent<GameUI>();

        // currentTurn = RandomizeFirstTurn();
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
    private PlayerToken RandomizeFirstTurn()
    {
        int roll = UnityEngine.Random.Range(0, 100);
        return (roll < 50) ? PlayerToken.P1 : PlayerToken.P2;
    }

    public void TileClick(GameObject clicked)
    {
        // Get tile data for clicked tile
        Tile newClickedTile = BoardC.GetTileDataByPos(clicked.transform.position, board);

        // check if clicked tile has a piece owned by human player
        if (newClickedTile.contents == TileContents.Piece && newClickedTile.piece.player == humanPlayer)
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
                    new List<Vector2> { new Vector2(newClickedTile.x, newClickedTile.y) }
                );
                currentClicked = board.tiles[newClickedTile.y][newClickedTile.x];

                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isHighlighted },
                    true,
                    BoardC.PossibleMoves(board.tiles, currentClicked)
                );
            }
            // if clicked the thing that is currently clicked
            else if (new Vector3(newClickedTile.x, 0, newClickedTile.y) == new Vector3(currentClicked.x, 0, currentClicked.y))
            {
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    false,
                    new List<Vector2> { new Vector2(currentClicked.x, currentClicked.y) }
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
                    new List<Vector2> { new Vector2(currentClicked.x, currentClicked.y) }
                );

                // switch previously selected tile to newly selected tile (which has isClicked state of true)
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isClicked },
                    true,
                    new List<Vector2> { new Vector2(newClickedTile.x, newClickedTile.y) }
                );
                currentClicked = board.tiles[newClickedTile.y][newClickedTile.x];

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
        else if (currentClicked != null && currentHover.isHighlighted)
        {
            // remove all state from all tiles
            board.tiles = BoardC.ChangeTilesState(
                board.tiles,
                new List<TileState> { TileState.isAOE, TileState.isClicked, TileState.isHighlighted, TileState.isHovered },
                false
            );


            // take control away from player

            ExecuteMove(currentClicked, currentHover);
            currentClicked = null;
            currentHover = null;
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void TileHover(GameObject hovered)
    {
        if (hovered == null) return;

        Tile newHover = BoardC.GetTileDataByPos(hovered.transform.position, board);

        // if we are hovering on the same thing as before
        if (currentHover != null && new Vector3(newHover.x, 0, newHover.y) == new Vector3(currentHover.x, 0, currentHover.y))
            return;

        // on new hover remove all AOE markeres
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
                new List<Vector2> { new Vector2(newHover.x, newHover.y) }
            );
            currentHover = board.tiles[newHover.y][newHover.x];
        }

        // if we are hovering on a new thing
        // Set old hovered to not hovered
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isHovered },
            false,
            new List<Vector2> { new Vector2(currentHover.x, currentHover.y) }
        );

        // Set new Hovered to hovered
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isHovered },
            true,
            new List<Vector2> { new Vector2(newHover.x, newHover.y) }
        );
        currentHover = board.tiles[newHover.y][newHover.x];

        // check if it's a piece or element
        if (currentHover.contents == TileContents.Piece)
        {
            ui.spellView.Toggle(false);
            ui.pieceView.UpdateView(currentHover.piece);
        }
        else
        {
            Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles));
            ui.pieceView.Toggle(false);

            // if piece is clicked and there are elements in our path
            if (currentClicked != null && potentialSpell != null)
            {
                // TODO: display damage / effects that would be done if this piece would be selected

                // show stats of potential spell
                ui.spellView.UpdateView(potentialSpell);

                // show potenetial spell AOE
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isAOE },
                    true,
                    BoardC.CalculateAOEPatterns(potentialSpell.pattern, currentHover)
                );
            }
            else
                ui.ToggleAllUI(false);
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void ExecuteMove(Tile start, Tile end)
    {
        // Check for spell
        bool pathHasSpell = false;
        // this spell will need to be determined in a later phase
        Spell selectedSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles));
        if (selectedSpell != null) pathHasSpell = true;

        MovePhase(start, end, () => { Debug.Log("made it"); });

        if (!pathHasSpell)
        {

        }
        else
        {
            // !! each phase has a data section and an animation section
            // each phase should have an end event function that is called when its complete
            // if each phase is a function that thakes the next phase as a function then we can chain phases

            // -- Move Phase
            // move the piece graphically
            // update board with new positions
            // once piece graphic reaches destination cast spell

            // -- Cast Phase
            // if spell parameter is not null
            // calculate damage/deaths/effects caused by spell
            // update effected piece stats
            // play spell animation
            // remove/play death animations of newly dead pieces
            // update board to have correct pieces 
            // update tiles to have correct tile contents

            // -- Upkeep Phase
            // restore all elements to the field 
            // remove dead pieces from the board
            // calculate exp gained by piece that just moved

            // -- Level Up Phase
            // if a piece levels up
            // calculate new stats
            // play level up animation
            // play stat increment animation or display new stats

            // -- New Player Turn Phase
            // increment turn counter
            // switch current player token
            // give control to the correct player
            // wait for input
        }


        // Debug.Log(selectedSpell.name);
    }

    private void MovePhase(Tile start, Tile end, Action nextPhase)
    {
        // --- Data -- 
        Tile startTile = start.Clone();
        Tile endTile = end.Clone();

        Vector2 startPos = new Vector2(startTile.x, startTile.y);
        Vector2 endPos = new Vector2(endTile.x, endTile.y);

        // update piece data and contents state of start and end tiles
        TileContents newStartContents = (startTile.element != 'N') ? TileContents.Element : TileContents.Empty;
        board.tiles = BoardC.UpdatePieceDataOnTile(board.tiles, startPos, newStartContents, null);
        board.tiles = BoardC.UpdatePieceDataOnTile(board.tiles, endPos, TileContents.Piece, startTile.piece);

        // --- Graphics ---
        graphics.MovePieceGraphic(startPos, endPos, nextPhase);
    }
}

// BUGS:
/*
- spell UI fields bleed into each other
- player first turn move with iron piece moving forward 3 (recipe: YWB) displays that it will cast judgement which should actually cost (WWY)
    - caused by no checking douplicates in permutation function?
*/
