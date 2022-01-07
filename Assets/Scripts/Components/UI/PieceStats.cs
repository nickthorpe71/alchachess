using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Data;

public class PieceStats : MonoBehaviour
{
    public GameObject statsCanvas;

    public Image healthGreen;
    public Image healthRed;
    public TextMeshProUGUI effectText;

    public Image expBar;
    public TextMeshProUGUI level;

    private float updateSpeed = 0.25f;

    public void UpdateHealthUI(Piece piece, float previousHealth)
    {
        statsCanvas.gameObject.SetActive(true);
        float preDamageHealthPercent = previousHealth / piece.maxHealth;
        float postDamageHealthPercent = piece.health / piece.maxHealth;

        healthGreen.fillAmount = postDamageHealthPercent;
        healthRed.fillAmount = preDamageHealthPercent;

        StartCoroutine(UpdateBar(healthRed, preDamageHealthPercent, postDamageHealthPercent));
        UpdateEffect(piece.currentSpellEffect);
    }

    private void UpdateEffect(string effect)
    {
        effectText.text = effect;
    }

    public void UpdateExpUI(Piece piece, float previousExp, bool didGainLevel)
    {
        statsCanvas.gameObject.SetActive(true);
        // TODO:
        // float startLevel;
        // float endLevel;
        // 

        // current exp - previous level required exp / next level required exp - previous level required exp

        // if didGainLevel
        // startLevel = piece.level - 1

        // --- start exp ---
        // = previousExp - current level required exp

        float startExpPercent = previousExp / piece.maxHealth;
        float endExpPercent = piece.health / piece.maxHealth;

        level.text = piece.level.ToString();
        expBar.fillAmount = startExpPercent;

        StartCoroutine(UpdateBar(expBar, startExpPercent, endExpPercent));
    }

    IEnumerator UpdateBar(Image targetBar, float preDamageHealthPercent, float postDamageHealthPercent)
    {
        float preChangePercent = preDamageHealthPercent;
        float elapsed = 0f;

        yield return new WaitForSeconds(1);

        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            targetBar.fillAmount = Mathf.Lerp(preChangePercent, postDamageHealthPercent, elapsed / updateSpeed);
            yield return null;
        }

        targetBar.fillAmount = postDamageHealthPercent;

        yield return new WaitForSeconds(2);

        statsCanvas.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        statsCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
