using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour
{
    // Environment
    [SerializeField] private GameObject emptyEnvironment;
    [SerializeField] private GameObject fireEnvironment;
    [SerializeField] private GameObject waterEnvironment;
    [SerializeField] private GameObject plantEnvironment;
    [SerializeField] private GameObject rockEnvironment;
    private Dictionary<string, EnvironmentData> environmentDataMap = new Dictionary<string, EnvironmentData>()
    {
        ["FireEnvironment"] = new FireEnv(),
        ["WaterEnvironment"] = new WaterEnv(),
        ["PlantEnvironment"] = new PlantEnv(),
        ["RockEnvironment"] = new RockEnv(),
        ["EmptyEnvironment"] = null
    };

    // Cursor Effects
    [SerializeField] private Transform hoverMarker;
    [SerializeField] private GameObject hovered;
    [SerializeField] private Transform clickMarker;
    [SerializeField] private GameObject clicked;
    [SerializeField] private GameObject aoe;
    [SerializeField] private GameObject highlighted;

    private GameObject elementObj;
    private GameObject pieceObj;
    private Element element;
    private Piece piece;
    private EnvironmentData activeEnvironment;
    private Vector2 pos;

    // GENERAL
    public void Init(GameObject element, Vector2 pos)
    {
        elementObj = element;
        this.element = element.GetComponent<Element>();
        this.pos = pos;
        activeEnvironment = null;
    }
    public bool CanTraverse() => piece == null;
    public Vector2 GetPos() => pos;
    public Element GetElement() => element;
    public TileData GetData()
    {
        PieceData validatedPiece = HasPiece() ? piece.GetData() : null;

        return new TileData(
            activeEnvironment,
            pos,
            element.GetData(),
            validatedPiece
        );
    }
    private void Activate(GameObject toActivate)
    {
        if (!toActivate.activeSelf)
            toActivate.SetActive(true);
    }
    private void Deactivate(GameObject toDeactivate)
    {
        if (toDeactivate.activeSelf)
            toDeactivate.SetActive(false);
    }

    // PLAY INPUT EFFECTS
    public void Hover(bool deactivate = false)
    {
        if (!deactivate)
        {
            Activate(hovered);
            SetMarkerHeight(hoverMarker);
        }
        else
            Deactivate(hovered);
    }
    public void Click(bool deactivate = false)
    {
        if (!deactivate)
        {
            Activate(clicked);
            SetMarkerHeight(clickMarker);
        }
        else
            Deactivate(clicked);
    }
    public void Highlight(bool deactivate = false)
    {
        if (!deactivate)
            Activate(highlighted);
        else
            Deactivate(highlighted);
    }
    public void AOE(bool deactivate = false)
    {
        if (!deactivate)
            Activate(aoe);
        else
            Deactivate(aoe);
    }
    public bool IsHighlighted() => highlighted.activeSelf;
    private void SetMarkerHeight(Transform marker)
    {
        if (piece != null)
        {
            float height = piece.GetComponent<BoxCollider>().size.y;
            marker.position = new Vector3(transform.position.x, height, transform.position.z);
        }
        else
            marker.position = new Vector3(transform.position.x, 1.2f, transform.position.z);
    }

    // ENVIRONMENTS
    public void ApplySpellToEnvironment(string spellColor)
    {
        DeactivateElement();

        switch (spellColor)
        {
            case "Red":
                FireEnvironment();
                break;
            case "Blue":
                WaterEnvironment();
                break;
            case "Green":
                PlantEnvironment();
                break;
            case "Yellow":
                RockEnvironment();
                break;
            case "Black":
                NullifyEnvironment();
                break;
            case "White":
                NullifyEnvironment();
                break;
            default:
                break;
        }
    }
    private void FireEnvironment()
    {
        if (emptyEnvironment.activeSelf)
            SwapEnvironments(emptyEnvironment, fireEnvironment);
        else if (fireEnvironment.activeSelf)
            return;
        else if (waterEnvironment.activeSelf)
            SwapEnvironments(waterEnvironment, emptyEnvironment);
        else if (plantEnvironment.activeSelf)
            SwapEnvironments(plantEnvironment, fireEnvironment);
        else if (rockEnvironment.activeSelf)
            return;
    }
    private void WaterEnvironment()
    {
        if (emptyEnvironment.activeSelf)
            SwapEnvironments(emptyEnvironment, waterEnvironment);
        else if (fireEnvironment.activeSelf)
            SwapEnvironments(fireEnvironment, emptyEnvironment);
        else if (waterEnvironment.activeSelf)
            return;
        else if (plantEnvironment.activeSelf)
            return;
        else if (rockEnvironment.activeSelf)
            SwapEnvironments(rockEnvironment, waterEnvironment);
    }
    private void PlantEnvironment()
    {
        if (emptyEnvironment.activeSelf)
            SwapEnvironments(emptyEnvironment, plantEnvironment);
        else if (fireEnvironment.activeSelf)
            return;
        else if (waterEnvironment.activeSelf)
            SwapEnvironments(waterEnvironment, plantEnvironment);
        else if (plantEnvironment.activeSelf)
            return;
        else if (rockEnvironment.activeSelf)
            SwapEnvironments(rockEnvironment, emptyEnvironment);
    }
    private void RockEnvironment()
    {
        if (emptyEnvironment.activeSelf)
            SwapEnvironments(emptyEnvironment, rockEnvironment);
        else if (fireEnvironment.activeSelf)
            SwapEnvironments(fireEnvironment, rockEnvironment);
        else if (waterEnvironment.activeSelf)
            return;
        else if (plantEnvironment.activeSelf)
            SwapEnvironments(plantEnvironment, emptyEnvironment);
        else if (rockEnvironment.activeSelf)
            return;
    }
    private void NullifyEnvironment()
    {
        new List<GameObject>(){
            fireEnvironment,
            waterEnvironment,
            plantEnvironment,
            rockEnvironment
        }.ForEach(env => Deactivate(env));

        Activate(emptyEnvironment);
        activeEnvironment = null;
    }
    private void SwapEnvironments(GameObject toDeactivate, GameObject toActivate)
    {
        Deactivate(toDeactivate);
        Activate(toActivate);
        activeEnvironment = environmentDataMap[toActivate.tag];
    }
    public bool HasActiveEnvironment()
    {
        bool hasActiveElement = new List<GameObject>(){
            fireEnvironment,
            waterEnvironment,
            plantEnvironment,
            rockEnvironment
        }.Aggregate(false, (any, val) => any || val.activeSelf);
        return hasActiveElement;
    }
    public bool EnvironmentDestroysOccupant() => activeEnvironment.destroysOccupant;
    public bool IsImmuneToElement(Element element)
    {
        if (element.GetColor() == "Red" && rockEnvironment.activeSelf)
            return true;
        if (element.GetColor() == "Blue" && plantEnvironment.activeSelf)
            return true;
        return false;
    }

    // PIECE
    public void SetPiece(Piece piece)
    {
        this.piece = piece;
        if (piece != null)
            pieceObj = piece.gameObject;
    }
    public Piece GetPiece() => piece;
    public void KillPiece()
    {
        piece.Kill();
        piece = null;
    }
    public void TransferPiece(Tile to, bool warp = true)
    {
        to.SetPiece(piece);
        piece.Move(startPos: pos, endTile: to, warp);
        piece = null;
    }
    public bool HasPlayersPiece(GenericPlayer player) => HasPiece() && piece.IsGold() == player.isGoldSide;
    public bool HasPiece() => piece != null;

    // ELEMENT
    public void ActivateElement()
    {
        if (!HasActiveEnvironment() && !HasPiece())
            element.GetComponent<Element>().Activate();
    }
    public void DeactivateElement()
    {
        element.GetComponent<Element>().Deactivate(playAnim: false);
    }
    public bool HasActiveElement() => element.GetComponent<Element>().IsActive();
}
