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
    public TextAsset masterSpellList;

    // GameState
    [NonSerialized] public Board board;
    [NonSerialized] public int turnCount = 1;
    [NonSerialized] public PlayerToken currentPlayer = PlayerToken.P1;
    private Tile currentHover = null;
    private Tile currentClicked = null;
    private bool humanCanInput;

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

        board.tiles[7][3].piece.level = 4; // temp to test leveling up

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

            Spell spell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(currentClicked, currentHover, board.tiles));

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
                float colorMod = SpellC.ColorMod(currentClicked.piece.element, "N", potentialSpell.color);
                ui.spellView.UpdateView(potentialSpell, currentClicked.piece, colorMod);

                // show potential spell AOE
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

    public void ExecuteMove(Tile start, Tile end, Spell spell)
    {
        humanCanInput = false;
        currentClicked = null;
        currentHover = null;
        MovePhase(start, end, spell);

        // !! each phase has a data section and an animation section
        // each phase should have an end event function that is called when its complete
        // if each phase is a function that takes the next phase as a function then we can chain phases

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
        // calculate effects
        // if a piece dies from effects then add them to dead pieces list

        // -- Level Up Phase
        // calculate exp gained by piece that just moved
        // if a piece levels up
        // calculate new stats
        // play level up animation
        // play stat increment animation or display new stats

        // -- Next Turn Phase
        // increment turn counter
        // switch current player token
        // give control to the correct player
        // wait for input
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
        List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, caster);
        Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(board.tiles, aoeRange, BoardC.ChoosePlayerTargetForEffect(currentPlayer, effect));
        Dictionary<Vector2, Tile> targetsPostDmg = new Dictionary<Vector2, Tile>();

        foreach (KeyValuePair<Vector2, Tile> kvp in targetsPreDmg)
        {
            Tile tileCopy = kvp.Value.Clone();
            float colorMod = SpellC.ColorMod(caster.piece.element, tileCopy.piece.element, spell.color);
            tileCopy.piece.health += PieceC.HealthAdjust(spell.damage, caster.piece.power, effect, colorMod);
            tileCopy.piece.power += PieceC.PowerAdjust(spell.damage, caster.piece.power, effect, colorMod);
            tileCopy.piece.currentSpellEffect = SpellC.DetermineLastingEffect(effect);
            tileCopy.piece.effectTurnsLeft = SpellC.DetermineEffectTurns(effect, colorMod, tileCopy.piece.effectTurnsLeft);
            tileCopy.piece.effectDamage
                = effect == "burn" ? SpellC.CalcBurn(SpellC.CalcDamage(spell.damage, caster.piece.power, colorMod))
                : effect == "poison" ? SpellC.CalcPoison(SpellC.CalcDamage(spell.damage, caster.piece.power, colorMod))
                : 0;
            tileCopy.piece.effectInflictor = caster.piece.label;

            board.tiles[tileCopy.y][tileCopy.x] = tileCopy;
            targetsPostDmg[kvp.Key] = tileCopy;
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

        // calculate multiple target defeat bonus
        int multiTargetBonus = deadTargets.Count;

        // calculate exp gained by piece that just moved
        int expGained = deadTargets.Values.Aggregate(0, (acc, tile) => acc + PieceC.ExpFromDefeatingOther(movedPiece.piece.level, tile.piece.level)) * multiTargetBonus;

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
            Piece leveledPiece = updatedExpPiece.Clone();
            leveledPiece.level += PieceC.CalcLevelFromExp(leveledPiece.experience);
            leveledPiece.power += leveledPiece.power / 5 * leveledPiece.level;
            leveledPiece.maxHealth += leveledPiece.maxHealth / 5 * leveledPiece.level;
            leveledPiece.health = leveledPiece.maxHealth;
            leveledPiece.moveDistance = Math.Min(6, leveledPiece.level % 3 == 0 ? leveledPiece.moveDistance + 1 : leveledPiece.moveDistance);
            board.tiles = PieceC.UpdatePieceOnTile(board.tiles, new Vector2(movedPiece.x, movedPiece.y), leveledPiece);
        }

        // --- Graphics ---
        graphics.PlayLevelPhaseAnims(() => NextTurnPhase(), board.tiles[movedPiece.y][movedPiece.x], startExp, startLevel);
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
            AIC.TakeTurn(board, this);
    }
}

// BUGS:
/*
- player first turn move with iron piece moving forward 3 (recipe: YWB) displays that it will cast judgement which should actually cost (WWY)
    - caused by no checking duplicates in permutation function?
*/
