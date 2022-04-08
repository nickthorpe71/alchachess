using UnityEngine;
using System.Collections.Generic;

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
    // public Environment environment { get; private set; }
    private Piece piece = null;

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

    public void ApplySpellToEnvironment(string spellColor)
    {
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
                BlackEnvironment();
                break;
            case "White":
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
            SwapEnvironments(rockEnvironment, plantEnvironment);
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
            return;
        else if (rockEnvironment.activeSelf)
            SwapEnvironments(rockEnvironment, plantEnvironment);
    }
    private void BlackEnvironment()
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

    public void Init(GameObject element, Vector2 pos)
    {
        this.element = element;
        this.pos = pos;
        activeEnvironment = null;
    }

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

    public void TransferPiece(Tile to)
    {
        to.SetPiece(piece);
        piece.Move(startPos: pos, endPos: to.pos);
        piece = null;
    }

    public bool CanTraverse() =>
        piece == null
        && !plantEnvironment.activeSelf
        && !rockEnvironment.activeSelf;

    public bool IsHighlighted() => highlighted.activeSelf;

    public bool HasActiveElement() => element.activeSelf;

    public bool HasPlayersPiece(GenericPlayer player) => HasPiece() && piece.isGold == player.isGoldSide;
    public bool HasPiece() => piece != null;

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
}
