using UnityEngine;
using TMPro;
using Data;
using Calc;

public class SpellView : MonoBehaviour, IToggle
{
    [Header("Values")]
    public TMP_Text spellName;
    public TMP_Text effect;
    public GameObject colorMatch;
    public TMP_Text colorMatchText;

    public void Toggle(bool isDisplayed)
    {
        gameObject.SetActive(isDisplayed);
    }

    public void UpdateView(Spell spell, Piece selectedPiece, float colorMod)
    {
        if (spell == null)
        {
            Toggle(false);
            return;
        }

        Toggle(true);
        colorMatch.SetActive(colorMod != 1f);
        colorMatchText.text = $"x{colorMod}";
        colorMatchText.color = (colorMod == 1.5f) ? Color.green : Color.red;
        spellName.text = spell.name;
        effect.text = SpellC.SpellEffectString(spell, selectedPiece.power, colorMod);
    }
}
