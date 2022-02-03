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
    public List<GameObject> activePieces = new List<GameObject>();
    public Dictionary<Vector2, GameObject> activeEnvironments = new Dictionary<Vector2, GameObject>();
    public Dictionary<Vector2, GameObject> elementGraphics = new Dictionary<Vector2, GameObject>();

    // Piece Movement
    [HideInInspector] public bool pieceIsMoving = false;
    private GameObject targetPiece;
    private Vector3 newPosition;
    private float moveSpeed = 3;
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
        if (tile.Contents == TileContents.Piece)
            InstantiatePiece(tile.Piece, tile.X, tile.Y);
        else if (tile.Contents == TileContents.Element)
            InstantiateElement(tile.Element, tile.X, tile.Y);
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
        newPiece.GetComponent<PieceStats>().UpdateUI(piece);

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
        elementGraphics[new Vector2(x, y)] = newElement;
    }

    public void RepopulateElements(Dictionary<Vector2, string> toRepopulate)
    {
        foreach (KeyValuePair<Vector2, string> kvp in toRepopulate)
            elementGraphics[kvp.Key].GetComponent<ElementGraphic>().Activate();
    }

    // --- Update ---
    public void UpdateTileGraphics(Tile[][] tiles)
    {
        GraphicsC.LoopTileGraphicsWithIndexes(tileGraphics, (graphic, x, y) =>
        {
            Tile currentTile = tiles[y][x];

            if (currentTile.IsAOE)
            {
                graphic.AOE();
                return;
            }
            else if (currentTile.IsClicked)
            {
                graphic.Click();
                return;
            }
            else if (currentTile.IsHovered)
            {
                graphic.Hover();
                return;
            }
            else if (currentTile.IsHighlighted)
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

    public void TogglePieceStatsUI(Vector2 piecePos, bool isActive)
    {
        PieceStats statsUI = GraphicsC.GetPieceStatsUI(piecePos, activePieces);
        statsUI.Toggle(isActive);
    }

    public void ToggleAllPieceStatsUI(bool isActive)
    {
        foreach (GameObject obj in activePieces)
        {
            PieceStats statsUI = GraphicsC.GetPieceStatsUI(new Vector2(obj.transform.position.x, obj.transform.position.z), activePieces);
            statsUI.Toggle(isActive);
        }
    }

    public void ShowPieceStats(Vector2 piecePos, Piece piece)
    {
        PieceStats statsUI = GraphicsC.GetPieceStatsUI(piecePos, activePieces);
        statsUI.UpdateUI(piece);
        statsUI.Toggle(true);
    }

    public void PlayCastAnims(
        Spell spell,
        List<Vector2> nonPieceTilesInRange,
        Action<Tile> upkeepPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg)
    {
        string spellAnimPath = GraphicsC.GetSpellAnimPrefabPath(spell);
        string castAnimPath = GraphicsC.GetCastAnimPrefabPath(spell);
        string environmentPrefabPath = nonPieceTilesInRange.Count > 0 ? GraphicsC.GetEnvironmentPrefabPath(spell) : null;

        StartCoroutine(CastAnims(castAnimPath, spellAnimPath, environmentPrefabPath, nonPieceTilesInRange, upkeepPhase, caster, targetsPreDmg, targetsPostDmg));
    }

    IEnumerator CastAnims(
        string castAnimPath,
        string spellAnimPath,
        string environmentPrefabPath,
        List<Vector2> nonPieceTilesInRange,
        Action<Tile> upkeepPhase,
        Tile caster,
        Dictionary<Vector2, Tile> targetsPreDmg,
        Dictionary<Vector2, Tile> targetsPostDmg)
    {
        // play cast animation
        GameObject castAnim = Instantiate(Resources.Load(castAnimPath) as GameObject);
        castAnim.transform.position = new Vector3(caster.X, 0.35f, caster.Y);
        Destroy(castAnim, 2);
        yield return new WaitForSeconds(1f);

        // play spell animation on each target
        foreach (Vector2 pos in targetsPreDmg.Keys)
        {
            GameObject spellAnim = Instantiate(Resources.Load(spellAnimPath) as GameObject);
            spellAnim.transform.position = new Vector3(pos.x, 0.7f, pos.y);
            yield return new WaitForSeconds(0.1f);
            Destroy(spellAnim, 2);
        }

        // if this spell affects the environment
        if (environmentPrefabPath != null)
            // create environment effect on all non piece tiles
            foreach (Vector2 pos in nonPieceTilesInRange)
            {

                GameObject environmentEffect = Instantiate(Resources.Load(environmentPrefabPath) as GameObject);
                environmentEffect.transform.position = new Vector3(pos.x, 0, pos.y);
                environmentEffect.GetComponent<EnvironmentEffect>().Raise();
                activeEnvironments[pos] = environmentEffect;
                if (elementGraphics.Keys.Contains(pos))
                    elementGraphics[pos].GetComponent<ElementGraphic>().Deactivate();
                yield return new WaitForSeconds(0.1f);
            }

        yield return new WaitForSeconds(0.25f);

        // display health reduction and effect application to correct pieces
        ReduceHealth(targetsPreDmg, targetsPostDmg);
        yield return new WaitForSeconds(0.5f);

        upkeepPhase(caster);
    }

    public void ReduceHealth(Dictionary<Vector2, Tile> targetsPreDmg, Dictionary<Vector2, Tile> targetsPostDmg)
    {
        foreach (KeyValuePair<Vector2, Tile> target in targetsPostDmg)
        {
            PieceStats pieceStatsUI = GraphicsC.GetPieceStatsUI(target.Key, activePieces);
            float previousHealth = targetsPreDmg[target.Key].Piece.health;
            pieceStatsUI.UpdateHealthUI(target.Value.Piece, previousHealth);
        }
    }

    public void PlayUpkeepAnims(
        Action nextTurnPhase,
        Tile caster,
        Dictionary<Vector2, Tile> deadTargets,
        Dictionary<Vector2, string> toRepopulate,
        List<Vector2> environmentsToRemove
        )
    {
        StartCoroutine(UpkeepAnims(nextTurnPhase, caster, deadTargets, toRepopulate, environmentsToRemove));
    }

    IEnumerator UpkeepAnims(
        Action nextTurnPhase,
        Tile caster,
        Dictionary<Vector2, Tile> deadTargets,
        Dictionary<Vector2, string> toRepopulate,
        List<Vector2> environmentsToRemove
        )
    {
        if (deadTargets.Count > 0)
        {
            foreach (KeyValuePair<Vector2, Tile> kvp in deadTargets)
            {
                // play death animations
                Vector3 positionIn3D = new Vector3(kvp.Key.x, 0, kvp.Key.y);
                GameObject deathAnim = Instantiate(Resources.Load("SpellAnims/DeathAnims/GenericDeathAnim") as GameObject);
                deathAnim.transform.position = positionIn3D;
                Destroy(deathAnim, 3);

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

        // remove environments
        if (environmentsToRemove.Count > 0)
        {
            float duration = 1.05f;
            foreach (Vector2 pos in environmentsToRemove)
            {
                GameObject env = activeEnvironments[pos];
                env.GetComponent<EnvironmentEffect>().Lower();
                activeEnvironments.Remove(pos);
                Destroy(env, duration);
            }
            yield return new WaitForSeconds(duration);
        }

        yield return new WaitForSeconds(1);
        RepopulateElements(toRepopulate);
        nextTurnPhase();
    }
}