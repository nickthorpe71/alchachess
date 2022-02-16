using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;
using Calc;

public class PieceStatsUI : MonoBehaviour
{
    [Header("References")]
    private Piece piece;
    public Image smallAvatar;
    public Image largeAvatar;
    public Image avatarBorder;
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
    private Image emptySlotImage;

    private void Start()
    {
        emptySlotImage = Resources.Load($"ElementImages/EmptySlot") as Image;
    }

    private void SetBorder(Piece startPiece)
    {
        avatarBorder = startPiece.color == PieceColor.White
            ? Resources.Load($"Avatars/WhiteBorder") as Image
            : Resources.Load($"Avatars/BlackBorder") as Image;
    }

    public void Init(Piece startPiece)
    {
        UpdateAvatar(startPiece.label);
        UpdatePower(startPiece.power);
        SetBorder(startPiece);
        UpdateUI(startPiece);
    }

    public void UpdateUI(Piece pieceUpdate)
    {
        UpdatePiece(pieceUpdate);
        // TODO: spell name will need to be pulled from spell name 
        // map once it's created
        UpdateSpellName(SpellC.GetSpellByRecipe(pieceUpdate.currentRecipe).name);
        PopulateElementSlots(pieceUpdate.currentRecipe);
    }

    public void PopulateElementSlots(string recipe)
    {
        for (int i = 0; i < elementsSmall.Length; i++)
        {
            if (recipe.Length == 0)
            {
                elementsSmall[i] = emptySlotImage;
                elementsLarge[i] = emptySlotImage;
                continue;
            }

            string nextLetter = recipe.Substring(0, 1);
            recipe = recipe.Substring(1, recipe.Length);
            Image elementImage = Resources.Load($"ElementImages/{nextLetter}Image") as Image;
            elementsSmall[i] = elementImage;
            elementsLarge[i] = elementImage;
        }
    }

    public void UpdateSpellName(string spellName)
    {
        spellNameText.text = spellName;
    }

    public void UpdateAvatar(PieceLabel label)
    {
        Image newAvatar = Resources.Load($"ElementImages/{nameof(label)}Image") as Image;
        smallAvatar = newAvatar;
        largeAvatar = newAvatar;
    }

    public void UpdatePower(float power)
    {
        powerText.text = power.ToString();
    }

    public void UpdatePiece(Piece newPiece)
    {
        piece = newPiece;
    }

    public void ToggleGlow()
    {
        glow.SetActive(!glow.activeSelf);
    }

    public void ToggleGlow(bool isActive)
    {
        glow.SetActive(isActive);
    }

    public void ToggleLargeStatsPane()
    {
        largeStatsPane.SetActive(!largeStatsPane.activeSelf);
    }

    public void ToggleLargeStatsPane(bool isActive)
    {
        largeStatsPane.SetActive(isActive);
    }

    public void UpdateHealth(Piece piece, float previousHealth)
    {
        float preDamageHealthPercent = previousHealth / piece.maxHealth;
        float postDamageHealthPercent = piece.health / piece.maxHealth;

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

        healthText.text = $"{piece.health} / {piece.maxHealth}";
    }

    IEnumerator UpdateBar(Image targetBar, float startPercent, float endPercent)
    {
        float preChangePercent = startPercent;
        float elapsed = 0f;

        yield return new WaitForSeconds(1);

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
