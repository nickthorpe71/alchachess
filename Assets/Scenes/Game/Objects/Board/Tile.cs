using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private GameObject emptyEnvironment;
    private GameObject fireEnvironment;
    private GameObject waterEnvironment;
    private GameObject plantEnvironment;
    private GameObject rockEnvironment;
    private List<GameObject> environments;

    private GameObject hovered;
    private GameObject clicked;
    private GameObject aoe;
    private GameObject highlighted;
    private List<GameObject> effects;

    public GameObject element { get; private set; }
    public Environment environment { get; private set; }

    private void Awake()
    {
        hovered = InstantiateEffect("Hovered");
        clicked = InstantiateEffect("Clicked");
        aoe = InstantiateEffect("AoE");
        highlighted = InstantiateEffect("Highlighted");
        effects = new List<GameObject> { hovered, clicked, aoe, highlighted };
        ResetEffects();

        emptyEnvironment = InstantiateEnvironment("EmptyEnvironment");
        fireEnvironment = InstantiateEnvironment("FireEnvironment");
        waterEnvironment = InstantiateEnvironment("WaterEnvironment");
        plantEnvironment = InstantiateEnvironment("PlantEnvironment");
        rockEnvironment = InstantiateEnvironment("RockEnvironment");
        environments = new List<GameObject> { emptyEnvironment, fireEnvironment, waterEnvironment, plantEnvironment, rockEnvironment };
        ResetEnvironments();
    }

    public void Hover()
    {
        ControlledActivate(hovered, effects);
    }
    public void Click()
    {
        ControlledActivate(clicked, effects);
    }
    public void Highlight()
    {
        ControlledActivate(highlighted, effects);
    }
    public void AOE()
    {
        ControlledActivate(aoe, effects);
    }

    public void FireEnvironment()
    {
        ControlledActivate(fireEnvironment, environments);
    }
    public void WaterEnvironment()
    {
        ControlledActivate(fireEnvironment, environments);
    }
    public void PlantEnvironment()
    {
        ControlledActivate(fireEnvironment, environments);
    }
    public void RockEnvironment()
    {
        ControlledActivate(fireEnvironment, environments);
    }
    public void EmptyEnvironment()
    {
        ControlledActivate(fireEnvironment, environments);
    }

    public bool CanTraverse() => environment == null || !environment.isTraversable;

    public void Init(GameObject element)
    {
        this.element = element;
    }

    private GameObject InstantiateEffect(string effectName)
    {
        Vector3 pos = new Vector3(transform.position.x, 0.27f, transform.position.z);
        return Instantiate(Resources.Load($"Tile/TileEffect/{effectName}") as GameObject, pos, Quaternion.identity);
    }

    private GameObject InstantiateEnvironment(string envName)
    {
        return Instantiate(Resources.Load($"Tile/Environment/{envName}") as GameObject, transform.position, Quaternion.identity);
    }

    private void DeactivateAll(List<GameObject> toDeactivate)
    {
        toDeactivate.ForEach(obj => ControlledDeactivate(obj));
    }

    private void ControlledActivate(GameObject toActivate, List<GameObject> toDeactivate)
    {
        toDeactivate.Remove(toActivate);
        DeactivateAll(toDeactivate);

        if (!toActivate.activeSelf)
            toActivate.SetActive(true);
    }

    private void ControlledDeactivate(GameObject toDeactivate)
    {
        if (toDeactivate.activeSelf)
            toDeactivate.SetActive(false);
    }

    public void ResetEffects()
    {
        DeactivateAll(effects);
    }

    public void ResetEnvironments()
    {
        ControlledActivate(emptyEnvironment, environments);
    }
}
