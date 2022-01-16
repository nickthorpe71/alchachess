using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;
using Data;
using Calc;

public class PieceStats : MonoBehaviour
{
    public GameObject statsCanvas;

    public TextMeshProUGUI characterName;

    public Image healthGreen;
    public Image healthRed;
    public TextMeshProUGUI healthTxt;

    public TextMeshProUGUI effectText;

    public Image expBar;
    public TextMeshProUGUI level;

    private float updateSpeed = 0.25f;

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
        level.text = piece.level.ToString();
        expBar.fillAmount = PieceC.ExpAsPercent(piece.experience, piece.level);
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

        // update level as it hasn't been initialized
        level.text = piece.level.ToString();

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

    public void UpdateExpUI(Tile pieceTile, float startExp, float startLevel, Action<Vector3> playLevelUpAnim, float previousHealth)
    {
        statsCanvas.gameObject.SetActive(true);
        Piece piece = pieceTile.piece;

        // set ui to show start exp and start level
        level.text = startLevel.ToString();

        float startExpPercent = PieceC.ExpAsPercent(startExp, startLevel);
        expBar.fillAmount = startExpPercent;

        float endExpPercent = PieceC.ExpAsPercent(piece.experience, piece.level);
        float levelsToGain = piece.level - startLevel;

        if (levelsToGain == 0)
            StartCoroutine(UpdateBar(expBar, startExpPercent, endExpPercent, true));
        else
        {
            StartCoroutine(LevelUpRoutine(startLevel, piece.level, startExpPercent, endExpPercent, new Vector3(pieceTile.x, 0, pieceTile.y), playLevelUpAnim));
            UpdateHealthUI(piece, previousHealth);
        }

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

    IEnumerator LevelUpRoutine(float startLevel, float endLevel, float startExpPercent, float endExpPercent, Vector3 piecePos, Action<Vector3> playLevelUpAnim)
    {
        float currentLevel = startLevel;

        // bring bar up to 100%
        yield return StartCoroutine(UpdateBar(expBar, startExpPercent, 1, false));
        currentLevel++;
        level.text = currentLevel.ToString();
        playLevelUpAnim(piecePos);
        yield return new WaitForSeconds(1f);

        // play animations for all intermediate levels
        while (currentLevel < endLevel)
        {
            yield return StartCoroutine(UpdateBar(expBar, 0, 1, false));
            currentLevel++;
            level.text = currentLevel.ToString();
            playLevelUpAnim(piecePos);
            yield return new WaitForSeconds(1f);
        }

        // animate remaining percent on bar
        yield return StartCoroutine(UpdateBar(expBar, 0, endExpPercent, false));
        yield return new WaitForSeconds(2);
        statsCanvas.SetActive(false);
    }

    private void LateUpdate()
    {
        statsCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
