using UnityEngine;

public class TileGraphic : MonoBehaviour
{
    private GameObject hovered;
    private GameObject clicked;
    private GameObject aoe;
    private GameObject highlighted;

    public bool isBlackTile;

    private void Awake()
    {
        // instantiate all effects on top of tile
        hovered = InstantiateEffect("Hovered");
        clicked = InstantiateEffect("Clicked");
        aoe = InstantiateEffect("AoE");
        highlighted = InstantiateEffect("Highlighted");

        DeactivateAllEffects();
    }

    private GameObject InstantiateEffect(string effectName)
    {
        GameObject newEffect = Instantiate(Resources.Load($"TileEffects/{effectName}") as GameObject);
        newEffect.transform.position = new Vector3(gameObject.transform.position.x, 0.27f, gameObject.transform.position.z);

        if (isBlackTile)
        {
            //TODO: loop through children and reduce alpha by 40%
        }

        return newEffect;
    }

    private void DeactivateAllEffects()
    {
        hovered.SetActive(false);
        clicked.SetActive(false);
        aoe.SetActive(false);
        highlighted.SetActive(false);
    }

    public void Reset()
    {
        DeactivateAllEffects();
    }

    public void Hover()
    {
        DeactivateAllEffects();
        hovered.SetActive(true);
    }

    public void Click()
    {
        DeactivateAllEffects();
        clicked.SetActive(true);
    }

    public void Highlight()
    {
        DeactivateAllEffects();
        highlighted.SetActive(true);
    }

    public void AOE()
    {
        DeactivateAllEffects();
        aoe.SetActive(true);
    }
}
