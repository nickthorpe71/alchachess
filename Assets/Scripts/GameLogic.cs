using UnityEngine;
using System;

using Actions;
using Calc;
using Data;

public class GameLogic : MonoBehaviour
{
    // Data
    private PlayerToken humanPlayer = PlayerToken.P1;
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

        currentTurn = RandomizeFirstTurn();
    }

    private void Start()
    {
        graphics.CollectTileGraphics();
    }

    void Update()
    {
        Player.HandleInput(this);
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

        // check if clicked tile has an element or is a human player tile
        if (newClickedTile.contents == TileContents.Piece && newClickedTile.piece.player == humanPlayer)
        {
            // Reset temporary states
            board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isHighlighted);
            board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isAOE);

            // if nothing currently clicked
            if (currentClicked == null)
            {
                board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, newClickedTile.x, newClickedTile.y, TileState.isClicked, true);
                currentClicked = board.tiles[newClickedTile.y][newClickedTile.x];

                // update highlight data
                board.tiles = BoardC.AddHighlightData(board.tiles, currentClicked);
            }
            // if clicked the thing that is currently clicked
            else if (new Vector3(newClickedTile.x, 0, newClickedTile.y) == new Vector3(currentClicked.x, 0, currentClicked.y))
            {
                board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, currentClicked.x, currentClicked.y, TileState.isClicked, false);
                currentClicked = null;
            }
            // if we clicked a new piece
            else
            {
                // switch isClicked state on previously selected tile to false
                board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, currentClicked.x, currentClicked.y, TileState.isClicked, false);

                // switch previously selected tile to newly selected tile (which has isClicked state of true)
                board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, newClickedTile.x, newClickedTile.y, TileState.isClicked, true);
                currentClicked = board.tiles[newClickedTile.y][newClickedTile.x];

                // update highlight data
                board.tiles = BoardC.AddHighlightData(board.tiles, currentClicked);
            }
        }
        else if (currentClicked.contents == TileContents.Element)
        {
            if (currentClicked == null)
                return;
            // cast spells
            Debug.Log("SPELLS!");
        }

        graphics.UpdateTileGraphics(board.tiles);
    }

    public void TileHover(GameObject hovered)
    {
        if (hovered == null)
            return;

        Tile newHover = BoardC.GetTileDataByPos(hovered.transform.position, board);

        // if we are hovering on the same thing as before
        if (currentHover != null && new Vector3(newHover.x, 0, newHover.y) == new Vector3(currentHover.x, 0, currentHover.y))
            return;

        // if nothing has been hovered yet
        if (currentHover == null)
        {
            board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, newHover.x, newHover.y, TileState.isHovered, true);
            currentHover = board.tiles[newHover.y][newHover.x];
        }

        // if we are hovering on a new thing
        // Set old hovered to not hovered
        board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, currentHover.x, currentHover.y, TileState.isHovered, false);

        // Set new Hovered to hovered
        board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, newHover.x, newHover.y, TileState.isHovered, true);
        currentHover = board.tiles[newHover.y][newHover.x];

        // check if it's a piece or element
        if (currentHover.contents == TileContents.Piece)
        {
            ui.spellView.Toggle(false);
            ui.pieceView.UpdateView(currentHover.piece);
        }
        else if (currentHover.contents == TileContents.Element)
        {
            ui.pieceView.Toggle(false);
            // TODO: check which tiles should be set to AOE (pattern from calculated spell)

            // check if a piece is clicked and if so display spell
            if (currentClicked != null)
                ui.spellView.UpdateView(SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles)));
            else
                ui.spellView.Toggle(false);
        }
        else
        {
            ui.ToggleAllUI(false);
        }

        graphics.UpdateTileGraphics(board.tiles);
    }
}

// BUGS: 
/*
- potential spells stays displayed after it displays once
- spell UI fields bleed into each other
- player first turn move with iron piece moving forward 3 (recipe: YWB) displays that it will cast judgement which should actually cost (WWY)
    - caused by no checking douplicates in permutation function?
*/
