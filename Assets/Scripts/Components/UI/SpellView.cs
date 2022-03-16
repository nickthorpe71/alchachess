using UnityEngine;
using TMPro;
using Data;
using Calc;

public class SpellView : MonoBehaviour, IToggle
{
    [Header("Values")]
    public TMP_Text spellName;
    public TMP_Text effect;

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void Toggle(bool isDisplayed)
    {
        gameObject.SetActive(isDisplayed);
    }

    public void UpdateView(Spell spell, Piece selectedPiece)
    {
        if (spell == null)
        {
            Toggle(false);
            return;
        }

        Toggle(true);
        spellName.text = spell.Name;
        if (selectedPiece != null)
            effect.text = SpellC.SpellEffectString(spell, selectedPiece.Power);
    }
}
