using UnityEngine;
using System.Collections.Generic;

public class GameUI : MonoBehaviour
{
    // References
    public SpellView spellView;
    [SerializeField]
    private List<GameObject> p1PieceStatUIs;
    [SerializeField]
    private List<GameObject> p2PieceStatUIs;

    public void ToggleAllUI(bool isDisplayed)
    {
        spellView.Toggle(isDisplayed);
    }

    //
}
