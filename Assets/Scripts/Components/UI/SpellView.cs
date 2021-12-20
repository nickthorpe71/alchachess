using UnityEngine;
using TMPro;
using Data;

public class SpellView : MonoBehaviour, IToggle
{
    [Header("Values")]
    public TMP_Text spellName;
    public TMP_Text damage;
    public TMP_Text color;
    public TMP_Text effect;

    public void Toggle(bool isDisplayed)
    {
        gameObject.SetActive(isDisplayed);
    }

    public void UpdateView(Spell spell)
    {
        if (spell == null)
        {
            Toggle(false);
            return;
        }

        Toggle(true);
        spellName.text = spell.name;
        damage.text = spell.damage.ToString();
        color.text = spell.color;
        effect.text = spell.spellEffect;
    }

}
