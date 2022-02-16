using UnityEngine;
using System.Collections.Generic;
using System;
using Data;

public class GameUI : MonoBehaviour
{
    // References
    [SerializeField]
    private SpellView spellView;
    [SerializeField]
    private Dictionary<Guid, PieceStatsUI> p1PieceStatUIs;
    [SerializeField]
    private Dictionary<Guid, PieceStatsUI> p2PieceStatUIs;

    // --- SpellUI --- \\

    public void ToggleSpellUI()
    {
        spellView.Toggle();
    }
    public void ToggleSpellUI(bool isDisplayed)
    {
        spellView.Toggle(isDisplayed);
    }

    public void UpdateSpellUI(Spell spell, Piece piece, float colorMod)
    {
        spellView.UpdateView(spell, piece, colorMod);
    }

    // --- PieceStatsUI --- \\

    public void InitPieceStatsUI(List<Piece> p1Pieces, List<Piece> p2Pieces)
    {

    }

    public void TogglePieceUIPane(PlayerToken player, Guid pieceGuid)
    {
        GetPieceUIByPlayer(player)[pieceGuid].ToggleLargeStatsPane();
    }
    public void TogglePieceUIPane(PlayerToken player, Guid pieceGuid, bool isOpen)
    {
        GetPieceUIByPlayer(player)[pieceGuid].ToggleLargeStatsPane(isOpen);
    }

    public void TogglePieceUIGlow(PlayerToken player, Guid pieceGuid)
    {
        GetPieceUIByPlayer(player)[pieceGuid].ToggleGlow();
    }
    public void TogglePieceUIGlow(PlayerToken player, Guid pieceGuid, bool isOpen)
    {
        GetPieceUIByPlayer(player)[pieceGuid].ToggleGlow(isOpen);
    }

    // Calc (need to move to own script)
    private Dictionary<Guid, PieceStatsUI> GetPieceUIByPlayer(PlayerToken player) => player == PlayerToken.P1 ? p1PieceStatUIs : p2PieceStatUIs;
}
