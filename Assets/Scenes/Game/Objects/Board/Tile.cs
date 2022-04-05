using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject emptyEnvironment;
    [SerializeField] private GameObject fireEnvironment;
    [SerializeField] private GameObject waterEnvironment;
    [SerializeField] private GameObject plantEnvironment;
    [SerializeField] private GameObject rockEnvironment;

    [SerializeField] private Transform hoverMarker;
    [SerializeField] private GameObject hovered;
    [SerializeField] private Transform clickMarker;
    [SerializeField] private GameObject clicked;
    [SerializeField] private GameObject aoe;
    [SerializeField] private GameObject highlighted;

    public Vector2 pos { get; private set; }

    public GameObject element { get; private set; }
    public Environment environment { get; private set; }
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

    public void FireEnvironment()
    {
        Activate(fireEnvironment);
    }
    public void WaterEnvironment()
    {
        Activate(fireEnvironment);
    }
    public void PlantEnvironment()
    {
        Activate(fireEnvironment);
    }
    public void RockEnvironment()
    {
        Activate(fireEnvironment);
    }
    public void EmptyEnvironment()
    {
        Activate(fireEnvironment);
    }

    public void Init(GameObject element, Vector2 pos)
    {
        this.element = element;
        this.pos = pos;
    }

    public void SetPiece(Piece piece)
    {
        this.piece = piece;
    }

    public Piece GetPiece() => piece;

    public void TransferPiece(Tile to)
    {
        to.SetPiece(piece);
        Debug.Log(piece);
        piece.Move(to.pos);
    }

    public bool CanTraverse() => piece == null && (environment == null || !environment.isTraversable);

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
