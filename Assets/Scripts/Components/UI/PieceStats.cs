using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Data;

public class PieceStats : MonoBehaviour
{
    public GameObject statsCanvas;

    public TextMeshProUGUI characterName;

    public Image healthGreen;
    public Image healthRed;
    public TextMeshProUGUI healthTxt;

    public TextMeshProUGUI effectText;

    private float updateSpeed = 0.1f;

    public void Toggle(bool isActive)
    {
        statsCanvas.SetActive(isActive);
    }

    public void UpdateUI(Piece piece)
    {
        healthGreen.fillAmount = piece.health / piece.maxHealth;
        healthRed.fillAmount = piece.health / piece.maxHealth;
        healthTxt.text = $"{piece.health} / {piece.maxHealth}";
        characterName.text = piece.label.ToString();
        UpdateEffect(piece.currentSpellEffect);
        // TODO:
        // update power
        // update movement
    }

    public void UpdateHealthUI(Piece piece, float previousHealth)
    {
        statsCanvas.gameObject.SetActive(true);
        float preDamageHealthPercent = previousHealth / piece.maxHealth;
        float postDamageHealthPercent = piece.health / piece.maxHealth;

        healthGreen.fillAmount = postDamageHealthPercent;
        healthRed.fillAmount = preDamageHealthPercent;

        StartCoroutine(UpdateBar(healthRed, preDamageHealthPercent, postDamageHealthPercent, true));
        UpdateEffect(piece.currentSpellEffect);
    }

    private void UpdateEffect(string effect)
    {
        if (effect == "" || effect == "none" || effect == null)
        {
            effectText.gameObject.SetActive(false);
            return;
        }

        effectText.gameObject.SetActive(true);
        effectText.text = effect;
    }

    IEnumerator UpdateBar(Image targetBar, float startPercent, float endPercent, bool deactivateCanvasPost)
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

        statsCanvas.gameObject.SetActive(!deactivateCanvasPost);
        if (deactivateCanvasPost)
            yield return new WaitForSeconds(2);
    }

    private void LateUpdate()
    {
        statsCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
