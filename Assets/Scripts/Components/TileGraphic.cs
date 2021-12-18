using UnityEngine;

public class TileGraphic : MonoBehaviour
{
    private Material neutral;
    public Material hovered;
    public Material clicked;
    public Material aoe;
    public Material highlighted;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        neutral = mesh.material;
    }

    public void Reset()
    {
        SetMat(neutral);
    }

    public void Hover()
    {
        SetMat(hovered);
    }

    public void Click()
    {
        SetMat(clicked);
    }

    public void Highlight()
    {
        SetMat(highlighted);
    }

    public void AOE()
    {
        SetMat(aoe);
    }

    private void SetMat(Material mat)
    {
        mesh.material = mat;
    }
}
