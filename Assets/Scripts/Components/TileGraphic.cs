using UnityEngine;

public class TileGraphic : MonoBehaviour
{
    private GameObject hovered;
    private GameObject clicked;
    private GameObject aoe;
    private GameObject highlighted;

    private void Awake()
    {
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
        return newEffect;
    }

    private void DeactivateAllEffects(GameObject excluded = null)
    {
        if (excluded != hovered)
            ControlledDeactivate(hovered);
        if (excluded != clicked)
            ControlledDeactivate(clicked);
        if (excluded != aoe)
            ControlledDeactivate(aoe);
        if (excluded != highlighted)
            ControlledDeactivate(highlighted);
    }

    public void Reset()
    {
        DeactivateAllEffects();
    }

    public void Hover()
    {
        ControlledActivate(hovered);
    }

    public void Click()
    {
        ControlledActivate(clicked);
    }

    public void Highlight()
    {
        ControlledActivate(highlighted);
    }

    public void AOE()
    {
        ControlledActivate(aoe);
    }

    private void ControlledActivate(GameObject toActivate)
    {
        DeactivateAllEffects(toActivate);

        if (!toActivate.activeSelf)
            toActivate.SetActive(true);
    }

    private void ControlledDeactivate(GameObject toDeactivate)
    {
        if (toDeactivate.activeSelf)
            toDeactivate.SetActive(false);
    }
}
