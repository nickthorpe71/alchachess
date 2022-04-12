using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject emptyEnvironment;
    [SerializeField] private GameObject fireEnvironment;
    [SerializeField] private GameObject waterEnvironment;
    [SerializeField] private GameObject plantEnvironment;
    [SerializeField] private GameObject rockEnvironment;
    public Environment activeEnvironment { get; private set; }

    [SerializeField] private Transform hoverMarker;
    [SerializeField] private GameObject hovered;
    [SerializeField] private Transform clickMarker;
    [SerializeField] private GameObject clicked;
    [SerializeField] private GameObject aoe;
    [SerializeField] private GameObject highlighted;

    public Vector2 pos { get; private set; }
    public GameObject element { get; private set; }
    private Piece piece = null;

    // GENERAL
    public void Init(GameObject element, Vector2 pos)
    {
        this.element = element;
        this.pos = pos;
        activeEnvironment = null;
    }
    public bool CanTraverse() => piece == null;
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
                if (piece == null)
                    PlantEnvironment();
                break;
            case "Yellow":
                if (piece == null)
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
        activeEnvironment = toActivate.GetComponent<Environment>();
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

    // PIECE
    public void SetPiece(Piece piece)
    {
        this.piece = piece;
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
    public bool HasPlayersPiece(GenericPlayer player) => HasPiece() && piece.isGold == player.isGoldSide;
    public bool HasPiece() => piece != null;

    // ELEMENT
    public void ActivateElement()
    {
        if (!HasActiveEnvironment() && !HasPiece())
            element.GetComponent<Element>().Activate();
    }
    public void DeactivateElement()
    {
        element.GetComponent<Element>().Deactivate();
    }
    public bool HasActiveElement() => element.GetComponent<Element>().IsActive();
}
