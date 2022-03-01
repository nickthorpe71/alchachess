using Calc;
using Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    [InspectorName("References")]
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

    private GameUI ui;

    // Piece Movement
    [HideInInspector] public bool pieceIsMoving = false;
    private GameObject targetPiece;
    private Vector3 newPosition;
    private float moveSpeed = 3;

    // --- Init ---
    public void Init(GameUI _ui)
    {
        ui = _ui;
    }

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

    public void InstantiatePiece(Piece piece, int x, int y)
    {
        string path = PieceC.GetPathByLabel(piece.Label, piece.Color);
        Vector3 pos = new Vector3(x, 0, y);
        int rotationY = piece.Color == PieceColor.White ? 0 : 180;
        Vector3 rotation = new Vector3(0, rotationY, 0);

        GameObject newPiece = Instantiate(Resources.Load(path) as GameObject);
        newPiece.transform.position = pos;
        newPiece.transform.eulerAngles = rotation;
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

    private void InstantiateEnvironmentEffect(string path, Vector2 pos)
    {
        GameObject environmentEffect = Instantiate(Resources.Load(path) as GameObject);
        environmentEffect.transform.position = new Vector3(pos.x, 0, pos.y);
        environmentEffect.GetComponent<EnvironmentEffect>().Raise();
        activeEnvironments[pos] = environmentEffect;
        if (elementGraphics.Keys.Contains(pos))
            elementGraphics[pos].GetComponent<ElementGraphic>().Deactivate();
    }

    public void RepopulateElements(Board board)
    {
        foreach (KeyValuePair<Vector2, GameObject> kvp in elementGraphics)
            if (!kvp.Value.activeSelf && BoardC.GetTile(board, kvp.Key).Contents == TileContents.Element)
                kvp.Value.GetComponent<ElementGraphic>().Activate();

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
            targetPiece = null;
        }
    }

    // --- Actions ---

    public void ExecuteMove(MoveData moveData, Action nextTurnPhase)
    {
        StartCoroutine(ExecuteMoveRoutine(moveData, nextTurnPhase));
    }

    IEnumerator ExecuteMoveRoutine(MoveData moveData, Action nextTurnPhase)
    {
        // --- Move Phase --- \\
        MovePieceGraphic(moveData.PieceStart, moveData.PieceEnd);
        yield return new WaitForSeconds((moveData.PieceStart - moveData.PieceEnd).magnitude * (moveSpeed / 10f));

        // --- Cast Phase --- \\

        // show spell UI thats being cast
        ui.UpdateSpellUI(moveData.SpellCast, moveData.PieceMoved);

        // get aoe pattern
        List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(
            moveData.SpellCast.Pattern,
            BoardC.GetTile(moveData.BoardPostMove, moveData.PieceEnd),
            moveData.ActingPlayer
        );

        // get targets
        List<Tile> targets = BoardC.GetTilesWithPiecesInRange(moveData.BoardPostMove, aoeRange);

        // play cast animation
        PlayAnim((int)moveData.PieceEnd.x, (int)moveData.PieceEnd.y, 0.35f, GraphicsC.GetCastAnimPrefabPath(moveData.SpellCast));
        yield return new WaitForSeconds(1f);

        // play spell animation on each target
        foreach (Tile target in targets)
        {
            PlayAnim(target.X, target.Y, 0.7f, GraphicsC.GetSpellAnimPrefabPath(moveData.SpellCast));
            yield return new WaitForSeconds(0.05f);
        }

        // get non piece tiles in range
        List<Vector2> newEnvironments = BoardC.GetNewlyAddedEnvironments(moveData.BoardPreMove, moveData.BoardPostMove);

        // if there are non piece tiles in range
        if (newEnvironments.Count > 0)
            foreach (Vector2 pos in newEnvironments)
            {
                InstantiateEnvironmentEffect(GraphicsC.GetEnvironmentPrefabPath(moveData.SpellCast), pos);
                yield return new WaitForSeconds(0.05f);
            }

        // display health reduction and effect application to correct pieces
        ui.UpdatePieceHealthBars(moveData.BoardPreMove, moveData.BoardPostMove);
        yield return new WaitForSeconds(1.5f);

        // turn off spell UI
        ui.ToggleSpellUI(false);


        // --- Upkeep --- \\

        // destroy graphics for dead pieces
        List<Vector2> deadTargets = BoardC.GetDeadPiecesByComparison(moveData.BoardPreMove, moveData.BoardPostMove);
        if (deadTargets.Count > 0)
        {
            foreach (Vector2 pos in deadTargets)
            {
                // play death animations
                Vector3 positionIn3D = new Vector3(pos.x, 0, pos.y);
                GameObject deathAnim = Instantiate(Resources.Load("SpellAnims/DeathAnims/GenericDeathAnim") as GameObject);
                deathAnim.transform.position = positionIn3D;
                Destroy(deathAnim, 3);

                // remove pieces from active pieces
                activePieces = activePieces.Where(piece =>
                {
                    if (piece.transform.position != positionIn3D)
                        return true;
                    Destroy(piece.gameObject);
                    ui.UpdatePieceUIDeath(BoardC.GetTile(moveData.BoardPreMove, pos).Piece.Guid);
                    return false;
                }).ToList();
            }
        }

        // remove environments
        List<Vector2> environmentsToRemove = BoardC.GetRemovedContents(moveData.BoardPreMove, moveData.BoardPostMove, TileContents.Environment);
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

        yield return new WaitForSeconds(0.25f);
        RepopulateElements(moveData.BoardPostMove);

        nextTurnPhase();
    }

    public void MovePieceGraphic(Vector2 start, Vector2 end)
    {
        targetPiece = GraphicsC.GetPieceByPosition(activePieces, start);
        newPosition = new Vector3(end.x, 0, end.y);
        pieceIsMoving = true;
    }

    public void WipePieces()
    {
        activePieces.ForEach(piece => Destroy(piece));
        activePieces.Clear();
    }

    private void PlayAnim(int x, int y, float height, string path)
    {
        GameObject anim = Instantiate(Resources.Load(path) as GameObject);
        anim.transform.position = new Vector3(x, 0.35f, y);
        Destroy(anim, 3);
    }
}