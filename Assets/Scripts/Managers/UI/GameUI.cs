using UnityEngine;
using TMPro;
using Data;

public class GameUI : MonoBehaviour
{
    // References
    [Header("Spell View")]
    public GameObject spellView;
    public TMP_Text spellName;
    public TMP_Text spellDamage;
    public TMP_Text spellColor;
    public TMP_Text spellEffect;

    // --- Spell View Start ---
    public void ToggleSpellView(bool isOn = false)
    {
        spellView.SetActive(isOn);
    }

    public void UpdateSpellView(Spell spell)
    {
        if (spell == null)
        {
            ToggleSpellView(false);
            return;
        }

        ToggleSpellView(true);
        spellName.text = spell.name;
        spellDamage.text = spell.damage.ToString();
        spellColor.text = spell.color;
        spellEffect.text = spell.spellEffect;
    }

    // --- Spell View End ---
}
