using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject emptyEnvironment;
    [SerializeField] private GameObject fireEnvironment;
    [SerializeField] private GameObject waterEnvironment;
    [SerializeField] private GameObject plantEnvironment;
    [SerializeField] private GameObject rockEnvironment;
    private List<GameObject> environments;

    [SerializeField] private Transform hoverMarker;
    [SerializeField] private GameObject hovered;
    [SerializeField] private Transform clickMarker;
    [SerializeField] private GameObject clicked;
    [SerializeField] private GameObject aoe;
    [SerializeField] private GameObject highlighted;
    private List<GameObject> effects;

    private List<GameObject> emptyList = new List<GameObject>();

    public GameObject element { get; private set; }
    public Environment environment { get; private set; }
    private Piece piece = null;

    private void Awake()
    {
        effects = new List<GameObject> { hovered, aoe, highlighted };
        environments = new List<GameObject> { emptyEnvironment, fireEnvironment, waterEnvironment, plantEnvironment, rockEnvironment };
    }

    public void Hover()
    {
        if (clicked.activeSelf) return;

        Activate(hovered, emptyList);
        SetMarkerHeight(hoverMarker);
    }
    public void UnHover()
    {
        Deactivate(hovered);
    }
    public void Click()
    {
        if (clicked.activeSelf)
            Deactivate(clicked);
        else
            Activate(clicked, effects);

        SetMarkerHeight(clickMarker);
    }
    public void Highlight()
    {
        Activate(highlighted, effects);
    }
    public void AOE()
    {
        Activate(aoe, effects);
    }

    public void FireEnvironment()
    {
        Activate(fireEnvironment, environments);
    }
    public void WaterEnvironment()
    {
        Activate(fireEnvironment, environments);
    }
    public void PlantEnvironment()
    {
        Activate(fireEnvironment, environments);
    }
    public void RockEnvironment()
    {
        Activate(fireEnvironment, environments);
    }
    public void EmptyEnvironment()
    {
        Activate(fireEnvironment, environments);
    }

    public bool CanTraverse() => piece == null && (environment == null || !environment.isTraversable);

    public void Init(GameObject element)
    {
        this.element = element;
    }

    public void SetPiece(Piece piece)
    {
        this.piece = piece;
    }

    public Piece GetPiece() => piece;

    public bool HasActiveElement() => element.activeSelf;

    public bool HasPlayersPiece(GenericPlayer player) => piece != null && piece.isGold == player.isGoldSide;

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

    private void DeactivateAll(List<GameObject> toDeactivate)
    {
        toDeactivate.ForEach(obj => Deactivate(obj));
    }

    private void Activate(GameObject toActivate, List<GameObject> toDeactivate)
    {
        List<GameObject> deactivate = new List<GameObject>(toDeactivate);
        deactivate.Remove(toActivate);
        DeactivateAll(deactivate);

        if (!toActivate.activeSelf)
            toActivate.SetActive(true);
    }

    private void Deactivate(GameObject toDeactivate)
    {
        if (toDeactivate.activeSelf)
            toDeactivate.SetActive(false);
    }

    public void ResetEffects(bool removeClick = false)
    {
        DeactivateAll(effects);

        if (removeClick)
            Deactivate(clicked);
    }

    public void ResetEnvironments()
    {
        Activate(emptyEnvironment, environments);
    }
}
