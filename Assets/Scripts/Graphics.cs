using Calc;
using Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    public Material blackMat;
    public Material whiteMat;
    public TileGraphic[][] tileGraphics;
    private Dictionary<string, string> elementResourceMap = new Dictionary<string, string>()
    {
        ["G"] = "Elements/Green",
        ["R"] = "Elements/Red",
        ["D"] = "Elements/Dark",
        ["W"] = "Elements/White",
        ["B"] = "Elements/Blue",
        ["Y"] = "Elements/Yellow"
    };
    private List<GameObject> activePieces = new List<GameObject>();

    // Piece Movement
    private bool pieceIsMoving = false;
    private GameObject targetPiece;
    private Vector3 newPosition;
    private float moveSpeed = 2;
    private Action postMoveAction;


    // --- Init ---
    public void CollectTileGraphics()
    {
        tileGraphics = SortTileGraphics(GameObject.FindGameObjectsWithTag("Tile"));
    }

    public TileGraphic[][] SortTileGraphics(GameObject[] collectedGraphics)
    {
        TileGraphic[][] result = new TileGraphic[Const.BOARD_HEIGHT][];

        for (int i = 0; i < Const.BOARD_HEIGHT; i++)
            result[i] = new TileGraphic[Const.BOARD_WIDTH];

        foreach (GameObject graphic in collectedGraphics)
        {
            int x = (int)graphic.transform.position.x;
            int y = (int)graphic.transform.position.z;
            result[y][x] = graphic.GetComponent<TileGraphic>();
        }

        return result;
    }

    // --- Instantiation ---
    public void InstantiateInitialBoard(Data.Board board)
    {
        BoardC.LoopTiles(board.tiles, InitTileOccupant);
    }

    private void InitTileOccupant(Tile tile)
    {
        if (tile.contents == TileContents.Piece)
            InstantiatePiece(tile.piece, tile.x, tile.y);
        else if (tile.contents == TileContents.Element)
            InstantiateElement(tile.element, tile.x, tile.y);
    }

    private void InstantiatePiece(Piece piece, int x, int y)
    {
        string path = PieceC.GetPathByLabel(piece.label);
        Vector3 pos = new Vector3(x, 0, y);
        int rotationY = piece.color == PieceColor.White ? 180 : 0;
        Vector3 rotation = new Vector3(0, rotationY, 0);

        GameObject newPiece = Instantiate(Resources.Load(path) as GameObject);
        newPiece.transform.position = pos;
        newPiece.transform.eulerAngles = rotation;

        newPiece.GetComponent<SetBaseColor>().SetColor(piece.color == PieceColor.White ? whiteMat : blackMat);

        activePieces.Add(newPiece);
    }

    private void InstantiateElement(string element, int x, int y)
    {
        string path = "Elements/" + element;
        Vector3 pos = new Vector3(x, 0.5f, y);
        GameObject newElement = Instantiate(Resources.Load(path) as GameObject);
        newElement.transform.position = pos;
        GameObject elementDestroyAnim = Resources.Load("Elements/DestroyAnimations/" + element + "DestroyAnim") as GameObject;
        ElementGraphic graphicComponent = newElement.GetComponent<ElementGraphic>();
        graphicComponent.destroyAnimPrefab = elementDestroyAnim;
        graphicComponent.graphics = this;
    }

    public void RepopulateElements(Dictionary<Vector2, string> toRepopulate)
    {
        foreach (KeyValuePair<Vector2, string> kvp in toRepopulate)
            InstantiateElement(kvp.Value, (int)kvp.Key.x, (int)kvp.Key.y);
    }

    // --- Update ---
    public void UpdateTileGraphics(Tile[][] tiles)
    {
        GraphicsC.LoopTileGraphicsWithIndexes(tileGraphics, (graphic, x, y) =>
        {
            Tile currentTile = tiles[y][x];

            if (currentTile.isAOE)
            {
                graphic.AOE();
                return;
            }
            else if (currentTile.isClicked)
            {
                graphic.Click();
                return;
            }
            else if (currentTile.isHovered)
            {
                graphic.Hover();
                return;
            }
            else if (currentTile.isHighlighted)
            {
                graphic.Highlight();
                return;
            }
            else
            {
                graphic.Reset();
            }
        });
    }

    public void PieceMovementUpdate()
    {
        if (!pieceIsMoving) return;

        float step = moveSpeed * Time.deltaTime;
        targetPiece.transform.position = Vector3.MoveTowards(targetPiece.transform.position, newPosition, step);

        if (targetPiece.transform.position == newPosition)
        {
            pieceIsMoving = false;
            postMoveAction();
            targetPiece = null;
            postMoveAction = null;
        }
    }

    // --- Actions ---
    public void MovePieceGraphic(Vector2 start, Vector2 end, Action postMove)
    {
        targetPiece = GraphicsC.GetPieceByPosition(activePieces, start);
        newPosition = new Vector3(end.x, 0, end.y);
        postMoveAction = postMove;
        pieceIsMoving = true;
    }

    public void PlayCastAnims(
        Spell spell,
        Action<Tile> upkeepPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg,
        List<Vector2> aoeRange)
    {
        string spellAnimPath = GraphicsC.GetSpellAnimPrefabPath(spell);
        string castAnimPath = GraphicsC.GetCastAnimPrefabPath(spell);

        StartCoroutine(CastAnims(castAnimPath, spellAnimPath, upkeepPhase, caster, targetsPreDmg, targetsPostDmg, aoeRange));
    }

    IEnumerator CastAnims(
        string castAnimPath,
        string spellAnimPath,
        Action<Tile> upkeepPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg,
        List<Vector2> aoeRange)
    {
        // play cast animation
        GameObject castAnim = Instantiate(Resources.Load(castAnimPath) as GameObject);
        castAnim.transform.position = new Vector3(caster.x, 0.35f, caster.y);
        Destroy(castAnim, 8);
        yield return new WaitForSeconds(1);

        // play spell animation on each target
        foreach (Vector2 pos in aoeRange)
        {
            GameObject spellAnim = Instantiate(Resources.Load(spellAnimPath) as GameObject);
            spellAnim.transform.position = new Vector3(pos.x, 0.7f, pos.y);
            yield return new WaitForSeconds(0.3f);
            Destroy(spellAnim, 8);
        }
        yield return new WaitForSeconds(0.25f);

        // display health reduction and effect application to correct pieces
        ReduceHealth(targetsPreDmg, targetsPostDmg);
        yield return new WaitForSeconds(2);

        upkeepPhase(caster);
    }

    public void ReduceHealth(Dictionary<Vector2, Tile> targetsPreDmg, Dictionary<Vector2, Tile> targetsPostDmg)
    {
        foreach (KeyValuePair<Vector2, Tile> target in targetsPostDmg)
        {
            GameObject pieceGraphic = GraphicsC.GetPieceByPosition(activePieces, target.Key);
            float previousHealth = targetsPreDmg[target.Key].piece.health;
            pieceGraphic.GetComponentInChildren<PieceStats>().UpdateHealthUI(target.Value.piece, previousHealth);
        }
    }

    public void PlayUpkeepAnims(
        Action<Dictionary<Vector2, Tile>, Tile> levelPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg,
        Dictionary<Vector2, Tile> deadTargets)
    {
        StartCoroutine(UpkeepAnims(levelPhase, caster, targetsPreDmg, targetsPostDmg, deadTargets));
    }

    IEnumerator UpkeepAnims(
        Action<Dictionary<Vector2, Tile>, Tile>
        levelPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg,
        Dictionary<Vector2, Tile> deadTargets)
    {
        ReduceHealth(targetsPreDmg, targetsPostDmg);
        yield return new WaitForSeconds(1);

        // TODO: show anim to add status effects and pass this function the currentEffects dict

        if (deadTargets.Count > 0)
        {
            foreach (KeyValuePair<Vector2, Tile> kvp in deadTargets)
            {
                // play death animations
                Vector3 positionIn3D = new Vector3(kvp.Key.x, 0, kvp.Key.y);
                GameObject deathAnim = Instantiate(Resources.Load("SpellAnims/DeathAnims/GenericDeathAnim") as GameObject);
                deathAnim.transform.position = positionIn3D;
                Destroy(deathAnim, 5);

                // remove pieces from active pieces
                activePieces = activePieces.Where(piece =>
                {
                    if (piece.transform.position != positionIn3D)
                    {
                        return true;
                    }
                    // destroy piece
                    Destroy(piece.gameObject);
                    return false;
                }).ToList();
            }
        }

        yield return new WaitForSeconds(1);
        levelPhase(deadTargets, caster);
    }

    public void PlayLevelPhaseAnims(Action nextTurnPhase, Tile pieceTile, float startExp, float startLevel)
    {
        StartCoroutine(LevelPhaseAnims(nextTurnPhase, pieceTile, startExp, startLevel));
    }

    IEnumerator LevelPhaseAnims(Action nextTurnPhase, Tile pieceTile, float startExp, float startLevel)
    {
        GameObject pieceGraphic = GraphicsC.GetPieceByPosition(activePieces, new Vector3(pieceTile.x, pieceTile.y));
        pieceGraphic.GetComponentInChildren<PieceStats>().UpdateExpUI(pieceTile, startExp, startLevel, PlayLevelUpAnim);
        yield return new WaitForSeconds(2f * (pieceTile.piece.level + 3));
        nextTurnPhase();
    }

    public void PlayLevelUpAnim(Vector3 pos)
    {
        GameObject levelUpAnim = Instantiate(Resources.Load("SpellAnims/LevelUpAnim") as GameObject);
        levelUpAnim.transform.position = pos;
        Destroy(levelUpAnim, 5);
    }
}