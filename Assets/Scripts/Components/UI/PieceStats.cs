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
    private float updateSpeed = 0.25f;

    public void UpdateStatsUI(Piece piece, float previousHealth)
    {
        statsCanvas.gameObject.SetActive(true);
        float preDamageHealthPercent = previousHealth / piece.maxHealth;
        float postDamageHealthPercent = piece.health / piece.maxHealth;

        healthGreen.fillAmount = postDamageHealthPercent;
        healthRed.fillAmount = preDamageHealthPercent;

        StartCoroutine(UpdateHealth(piece, preDamageHealthPercent, postDamageHealthPercent));
        UpdateEffect(piece.currentSpellEffect);
    }

    private void UpdateEffect(string effect)
    {
        effectText.text = effect;
    }

    IEnumerator UpdateHealth(Piece piece, float preDamageHealthPercent, float postDamageHealthPercent)
    {
        float preChangePercent = preDamageHealthPercent;
        float elapsed = 0f;

        yield return new WaitForSeconds(1);

        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            healthRed.fillAmount = Mathf.Lerp(preChangePercent, postDamageHealthPercent, elapsed / updateSpeed);
            yield return null;
        }

        healthRed.fillAmount = postDamageHealthPercent;

        yield return new WaitForSeconds(3);

        statsCanvas.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        statsCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
