using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Actions;
using Calc;
using Data;

public class GameLogic : MonoBehaviour
{
    // Data
    [SerializeField] public PlayerToken humanPlayer = PlayerToken.P1;
    [SerializeField] public PlayerToken aiPlayer = PlayerToken.P2;
    public TextAsset masterSpellList;

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

        SpellLoader.LoadAllSpells(masterSpellList);

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
            if (!humanCanInput)
                return;

            if (currentClicked.piece.currentSpellEffect == "frozen")
            {
                // TODO: need to display this message in UI
                Debug.Log("cannot move a frozen piece");
                return;
            }

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

        Tile newHover = BoardC.GetTileDataByPos(hovered.transform.position, board);

        // if we are hovering on the same thing as before
        if (currentHover != null && new Vector3(newHover.x, 0, newHover.y) == new Vector3(currentHover.x, 0, currentHover.y))
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

        graphics.ToggleAllPieceStatsUI(false);

        // Set new Hovered to hovered TileState
        board.tiles = BoardC.ChangeTilesState(
            board.tiles,
            new List<TileState> { TileState.isHovered },
            true,
            new List<Vector2> { new Vector2(newHover.x, newHover.y) }
        );
        currentHover = board.tiles[newHover.y][newHover.x];

        // if hovering a piece
        if (currentHover.contents == TileContents.Piece)
        {
            ui.spellView.Toggle(false);
            if (!graphics.pieceIsMoving)
                graphics.ShowPieceStats(new Vector2(currentHover.x, currentHover.y), currentHover.piece);
        }
        else // if hoverint an element
        {
            if (currentClicked == null) return;

            Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles, humanPlayer, currentPlayer));
            graphics.TogglePieceStatsUI(new Vector2(currentClicked.x, currentClicked.y), false);

            // if piece is clicked and there are elements in our path
            if (currentClicked != null && potentialSpell != null)
            {
                // show stats of potential spell
                float colorMod = SpellC.ColorMod(currentClicked.piece.element, "N", potentialSpell.color);
                ui.spellView.UpdateView(potentialSpell, currentClicked.piece, colorMod);

                // show potential spell AOE
                board.tiles = BoardC.ChangeTilesState(
                    board.tiles,
                    new List<TileState> { TileState.isAOE },
                    true,
                    BoardC.CalculateAOEPatterns(potentialSpell.pattern, currentHover, currentClicked.piece.player)
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
        humanCanInput = false;
        currentClicked = null;
        currentHover = null;
        MovePhase(start, end, spell);
    }

    private void MovePhase(Tile start, Tile end, Spell spell)
    {
        // --- Data ---
        Tile startTile = start.Clone();
        Tile endTile = end.Clone();

        Vector2 startPos = new Vector2(startTile.x, startTile.y);
        Vector2 endPos = new Vector2(endTile.x, endTile.y);

        // update piece data and contents state of start and end tiles
        board.tiles = BoardC.MapTilesBetween(board.tiles, startPos, endPos, (tile, x, y) =>
        {
            Vector2 pos = new Vector2(x, y);
            if (pos == endPos)
            {   // set end tile contents to piece and piece data to moved piece
                tile.contents = TileContents.Piece;
                tile.piece = startTile.piece;
            }
            else
            {   // for all other tiles set contents to empty and piece to null
                tile.contents = TileContents.Empty;
                tile.piece = null;
            }

            return tile;
        });

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

        // calculate damage and effects of spell
        Tile caster = board.tiles[end.y][end.x];
        string effect = spell.spellEffect;

        // apply damage and effects to pieces in range
        List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, caster, caster.piece.player);
        Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(board.tiles, aoeRange, BoardC.ChoosePlayerTargetForEffect(currentPlayer, effect));
        Dictionary<Vector2, Tile> targetsPostDmg = new Dictionary<Vector2, Tile>();

        foreach (KeyValuePair<Vector2, Tile> kvp in targetsPreDmg)
        {
            Piece piecePostSpell = PieceC.ApplySpellToPiece(caster.piece, kvp.Value.piece, spell);
            Tile tileWithNewPiece = board.tiles[(int)kvp.Key.y][(int)kvp.Key.x].Clone();
            tileWithNewPiece.piece = piecePostSpell;
            board.tiles[(int)kvp.Key.y][(int)kvp.Key.x] = tileWithNewPiece;
            targetsPostDmg[kvp.Key] = tileWithNewPiece;
        };

        // --- Graphics ---
        // play spell animation
        graphics.PlayCastAnims(spell, (caster) => UpkeepPhase(caster), caster, targetsPreDmg, targetsPostDmg, aoeRange);
    }


    public void UpkeepPhase(Tile movedPiece)
    {
        // --- Data ---
        // restore all elements to the field
        Dictionary<Vector2, string> toRepopulate = new Dictionary<Vector2, string>();
        board.tiles = BoardC.RepopulateElements(board.tiles, toRepopulate);

        // calculate effects and save a copy of effected pieces
        Dictionary<Vector2, StatusChange> currentEffects = PieceC.GetCurrentStatusEffects(board.tiles);

        // apply effects and record health pre and post effect
        Dictionary<Vector2, Tile> targetsPreDamage = new Dictionary<Vector2, Tile>();
        Dictionary<Vector2, Tile> targetsPostDamage = new Dictionary<Vector2, Tile>();
        board.tiles = BoardC.MapTiles(board.tiles, tileCopy =>
        {
            Vector2 pos = new Vector2(tileCopy.x, tileCopy.y);
            if (!currentEffects.ContainsKey(pos)) return tileCopy;

            // store pre damage targets
            targetsPreDamage[pos] = tileCopy;

            // apply status effect/damage
            tileCopy = PieceC.ApplyStatusEffects(tileCopy, currentEffects[pos]);

            // store post damage targets
            targetsPostDamage[pos] = tileCopy;
            return tileCopy;
        });

        // scan board for dead pieces
        Dictionary<Vector2, Tile> deadTargets = new Dictionary<Vector2, Tile>();
        board.tiles = BoardC.MapTiles(board.tiles, tile =>
        {
            if (tile.contents != TileContents.Piece || tile.piece.health > 0) return tile;
            else
            {
                // if found remove from board data and create list to 
                //send to graphics to be removed as well
                deadTargets[new Vector2(tile.x, tile.y)] = tile;
                return BoardC.RemovePiece(tile);
            }
        });

        // --- Graphics ---
        graphics.RepopulateElements(toRepopulate);

        // show effect animations and remove health and destroy newly dead targets
        graphics.PlayUpkeepAnims((deadTargets, movedPiece) => LevelUpPhase(deadTargets, movedPiece), movedPiece, targetsPreDamage, targetsPostDamage, deadTargets);
    }

    public void LevelUpPhase(Dictionary<Vector2, Tile> deadTargets, Tile movedPiece)
    {
        // if dead targets is null then no spell was cast
        // if dead targets length is 0 no exp is gained
        if (deadTargets == null || deadTargets.Count <= 0)
        {
            NextTurnPhase();
            return;
        }

        // --- Data ---
        // store start level and exp for graphics
        float startLevel = movedPiece.piece.level;
        float startExp = movedPiece.piece.experience;
        float startHealth = movedPiece.piece.health;

        // calculate multiple target defeat bonus
        int multiTargetBonus = deadTargets.Count;

        // calculate exp gained by piece that just moved
        int expGained = deadTargets.Values.ToList().Aggregate(0, (acc, tile) => acc + PieceC.ExpFromDefeatingOther(movedPiece.piece.level, tile.piece.level)) * Mathf.Max((multiTargetBonus / 2), 1);
        Debug.Log(expGained);

        // add exp to piece
        Piece updatedExpPiece = movedPiece.piece.Clone();
        updatedExpPiece.experience += expGained;
        board.tiles = PieceC.UpdatePieceOnTile(board.tiles, new Vector2(movedPiece.x, movedPiece.y), updatedExpPiece);

        // check if a piece levels up
        int expToLevel = PieceC.ExpForNextLevel(movedPiece.piece.level);
        bool pieceLeveled = (board.tiles[movedPiece.y][movedPiece.x].piece.experience >= expToLevel);

        // calculate new stats and level
        if (pieceLeveled)
        {
            Debug.Log("made it");
            Piece leveledPiece = updatedExpPiece.Clone();
            leveledPiece.level += PieceC.CalcLevelFromExp(leveledPiece.experience);
            leveledPiece.power += leveledPiece.power / 5 * leveledPiece.level;
            leveledPiece.maxHealth += leveledPiece.maxHealth / 5 * leveledPiece.level;
            leveledPiece.health = leveledPiece.maxHealth;
            leveledPiece.moveDistance = Math.Min(6, leveledPiece.level % 3 == 0 ? leveledPiece.moveDistance + 1 : leveledPiece.moveDistance);
            board.tiles = PieceC.UpdatePieceOnTile(board.tiles, new Vector2(movedPiece.x, movedPiece.y), leveledPiece);
        }

        // --- Graphics ---
        graphics.PlayLevelPhaseAnims(() => NextTurnPhase(), board.tiles[movedPiece.y][movedPiece.x], startExp, startLevel, startHealth);
    }

    public void NextTurnPhase()
    {
        // increment turn counter
        turnCount++;

        // switch current player token
        currentPlayer = PlayerC.SwitchPlayers(currentPlayer);

        // give control to the correct player
        humanCanInput = PlayerC.CanHumanInput(currentPlayer);

        // wait for input
        if (currentPlayer != humanPlayer)
            AIC.TakeTurn(board, this, 5); // difficulty is 1-5
    }
}

// BUGS:
/*
- killing a piece only gives exp if you kill multiple
- looks like movement isn't increasing with level
- if a piece dies from poison or burn an error is thrown
- ai can move frozen pieces
- putting cursor off board with piece selected throws error
- healing heals past max health
- elements are not respawning where a piece dies
- health bar doesn't update when gaining levels
- when health is 0.1321415 the piece doesn't die
- after level 6 gaining a level makes exp go down
*/

// Balance
/*
- eden to far red element first turn is OP
- heal needs to be buffed
- 
*/
