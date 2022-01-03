using UnityEngine;
using TMPro;
using Data;
using Calc;

public class SpellView : MonoBehaviour, IToggle
{
    [Header("Values")]
    public TMP_Text spellName;
    public TMP_Text color;
    public TMP_Text effect;
    public GameObject colorMatch;

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
        colorMatch.SetActive(colorMod == 1.5f);
        spellName.text = spell.name;
        color.text = SpellC.ColorToString(spell.color);
        effect.text = SpellC.SpellEffectString(spell.spellEffect, spell.damage, selectedPiece.power, colorMod);
    }
}
