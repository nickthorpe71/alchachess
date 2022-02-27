using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;
using Calc;

public class PieceStatsUI : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Guid guid;
    public Image smallAvatar;
    public Image largeAvatar;
    public Image smallAvatarBorder;
    public Image largeAvatarBorder;
    public Image[] elementsSmall;
    public Image[] elementsLarge;
    public Image healthFrontSmall;
    public Image healthBackSmall;
    public Image healthFrontLarge;
    public Image healthBackLarge;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI powerText;
    public GameObject glow;
    public GameObject largeStatsPane;

    private float updateSpeed = 0.75f;
    private Sprite emptySlotImage;
    private Sprite whiteBorder;
    private Sprite blackBorder;
    private Sprite deadAvatar;

    private void Awake()
    {
        emptySlotImage = Resources.Load<Sprite>($"UI/ElementImages/EmptySlot");
        whiteBorder = Resources.Load<Sprite>($"UI/PieceAvatars/BorderWhite");
        blackBorder = Resources.Load<Sprite>($"UI/PieceAvatars/BorderBlack");
        deadAvatar = Resources.Load<Sprite>($"UI/PieceAvatars/DeadAvatar");
    }

    public void Init(Piece startPiece)
    {
        gameObject.SetActive(true);
        UpdateAvatar(startPiece.Label);
        UpdatePower(startPiece.Power);
        SetBorder(startPiece.Color);
        UpdateUI(startPiece);
        UpdateHealthBar(startPiece.Health, startPiece.Health, startPiece.MaxHealth);
    }

    private void SetBorder(PieceColor color)
    {
        Sprite avatarBorder = color == PieceColor.White ? whiteBorder : blackBorder;
        largeAvatarBorder.sprite = avatarBorder;
        smallAvatarBorder.sprite = avatarBorder;
    }

    public void UpdateUI(Piece pieceUpdate)
    {
        UpdateGuid(pieceUpdate.Guid);
        // TODO: spell name will need to be pulled from spell name map once it's created
        Spell spell = SpellC.GetSpellByRecipe(pieceUpdate.CurrentRecipe);
        if (spell != null)
            UpdateSpellName(spell.Name);
        PopulateElementSlots(pieceUpdate.CurrentRecipe);
    }

    public void PopulateElementSlots(string recipe)
    {
        for (int i = 0; i < elementsSmall.Length; i++)
        {
            if (recipe.Length == 0)
            {
                elementsSmall[i].sprite = emptySlotImage;
                elementsLarge[i].sprite = emptySlotImage;
                continue;
            }

            string nextLetter = recipe.Substring(0, 1);
            recipe = recipe.Substring(1, recipe.Length);
            Sprite elementImage = Resources.Load<Sprite>($"UI/ElementImages/{nextLetter}Image");
            elementsSmall[i].sprite = elementImage;
            elementsLarge[i].sprite = elementImage;
        }
    }

    public void UpdateSpellName(string spellName)
    {
        spellNameText.text = spellName;
    }

    public void UpdateAvatar(PieceLabel label)
    {
        Sprite newAvatar = Resources.Load<Sprite>($"UI/PieceAvatars/{label.ToString()}Avatar");
        smallAvatar.sprite = newAvatar;
        largeAvatar.sprite = newAvatar;
    }

    public void DeadAvatar()
    {
        smallAvatar.sprite = deadAvatar;
        largeAvatar.sprite = deadAvatar;
    }

    public void UpdatePower(float power)
    {
        powerText.text = power.ToString();
    }

    public void UpdateGuid(Guid newGuid)
    {
        guid = newGuid;
    }

    public void ToggleGlow()
    {
        glow.SetActive(!glow.activeSelf);
    }

    public void ToggleGlow(bool isActive)
    {
        glow.SetActive(isActive);
    }

    // public void ToggleLargeStatsPane()
    // {
    //     largeStatsPane.SetActive(!largeStatsPane.activeSelf);
    // }

    public void ToggleLargeStatsPane(bool isActive)
    {
        largeStatsPane.SetActive(isActive);
    }

    public void UpdateHealthBar(float currentHealth, float previousHealth, float maxHealth)
    {
        float preDamageHealthPercent = previousHealth / maxHealth;
        float postDamageHealthPercent = currentHealth / maxHealth;

        // if damaged
        if (preDamageHealthPercent > postDamageHealthPercent)
        {
            healthFrontSmall.fillAmount = postDamageHealthPercent;
            healthBackSmall.fillAmount = preDamageHealthPercent;
            healthFrontLarge.fillAmount = postDamageHealthPercent;
            healthBackLarge.fillAmount = preDamageHealthPercent;

            StartCoroutine(UpdateBar(healthBackSmall, preDamageHealthPercent, postDamageHealthPercent));
            StartCoroutine(UpdateBar(healthBackLarge, preDamageHealthPercent, postDamageHealthPercent));

        } // if heal
        else if (preDamageHealthPercent < postDamageHealthPercent)
        {
            healthFrontSmall.fillAmount = preDamageHealthPercent;
            healthBackSmall.fillAmount = postDamageHealthPercent;
            healthFrontLarge.fillAmount = preDamageHealthPercent;
            healthBackLarge.fillAmount = postDamageHealthPercent;

            StartCoroutine(UpdateBar(healthFrontSmall, preDamageHealthPercent, postDamageHealthPercent));
            StartCoroutine(UpdateBar(healthFrontLarge, preDamageHealthPercent, postDamageHealthPercent));
        }

        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    IEnumerator UpdateBar(Image targetBar, float startPercent, float endPercent)
    {
        float preChangePercent = startPercent;
        float elapsed = 0f;

        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            targetBar.fillAmount = Mathf.Lerp(preChangePercent, endPercent, elapsed / updateSpeed);
            yield return null;
        }

        targetBar.fillAmount = endPercent;
        yield return new WaitForSeconds(1);
    }
}
