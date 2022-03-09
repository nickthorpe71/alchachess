using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Data;

public class PieceSelectItem : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI pieceName;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private Button selectButton;

    public void Init(Piece piece, Action<string> onClick)
    {
        string labelAsStr = piece.Label.ToString();
        Sprite newAvatar = Resources.Load<Sprite>($"UI/PieceAvatars/{labelAsStr}Avatar");

        avatar.sprite = newAvatar;
        pieceName.text = labelAsStr;
        health.text = piece.Health.ToString();
        power.text = piece.Power.ToString();
        selectButton.onClick.AddListener(delegate { onClick(labelAsStr); });
    }
}
