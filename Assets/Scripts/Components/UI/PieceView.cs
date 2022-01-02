using System;
using UnityEngine;
using TMPro;
using Data;

public class PieceView : MonoBehaviour, IToggle
{
    [Header("Values")]
    public TMP_Text pieceName;
    public TMP_Text level;
    public TMP_Text element;
    public TMP_Text player;
    public TMP_Text health;
    public TMP_Text attack;
    public TMP_Text moveDistance;
    public TMP_Text effect;

    public void Toggle(bool isDisplayed)
    {
        gameObject.SetActive(isDisplayed);
    }

    public void UpdateView(Piece piece)
    {
        if (piece == null)
        {
            Toggle(false);
            return;
        }

        Toggle(true);
        pieceName.text = Enum.GetName(typeof(PieceLabel), piece.label);
        level.text = piece.level.ToString();
        element.text = piece.element.ToString();
        player.text = Enum.GetName(typeof(PlayerToken), piece.player);
        health.text = piece.health.ToString();
        attack.text = piece.attack.ToString();
        moveDistance.text = piece.moveDistance.ToString();
        effect.text = piece.currentSpellEffect;
    }
}
