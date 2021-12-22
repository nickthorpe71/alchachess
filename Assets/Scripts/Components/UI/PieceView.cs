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

    public void Toggle(bool isDisplayed)
    {
        gameObject.SetActive(isDisplayed);
    }

    public void UpdateView(Piece peice)
    {
        if (peice == null)
        {
            Toggle(false);
            return;
        }

        Toggle(true);
        pieceName.text = Enum.GetName(typeof(PieceLabel), peice.label);
        level.text = peice.level.ToString();
        element.text = peice.element.ToString();
        player.text = Enum.GetName(typeof(PlayerToken), peice.player);
        health.text = peice.health.ToString();
        attack.text = peice.attack.ToString();
        moveDistance.text = peice.moveDistance.ToString();
    }
}
