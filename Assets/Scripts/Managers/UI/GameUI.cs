using UnityEngine;

public class GameUI : MonoBehaviour
{
    // References
    public SpellView spellView;
    public PieceView pieceView;

    public void ToggleAllUI(bool isDisplayed)
    {
        spellView.Toggle(isDisplayed);
        pieceView.Toggle(isDisplayed);
    }
}
