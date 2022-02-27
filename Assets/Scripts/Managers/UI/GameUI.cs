using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
using Calc;

public class GameUI : MonoBehaviour
{
    // References
    [SerializeField] private SpellView spellView;
    [SerializeField] private GameObject pieceSelectUI;
    [SerializeField] private List<PieceStatsUI> p1PieceStatUIs;
    [SerializeField] private List<PieceStatsUI> p2PieceStatUIs;
    private GameLogic logic;
    private Guid startGuid = Guid.NewGuid();
    private Guid currentOpenPieceDetails;
    private Guid currentGlow;

    private void Start()
    {
        logic = GetComponent<GameLogic>();
        currentOpenPieceDetails = startGuid;
        currentGlow = startGuid;
    }

    // --- SpellUI --- \\

    public void ToggleSpellUI(bool isDisplayed)
    {
        spellView.Toggle(isDisplayed);
    }

    public void UpdateSpellUI(Spell spell, Piece piece)
    {
        spellView.UpdateView(spell, piece);
    }

    // --- PieceStatsUI --- \\

    // public void InitPieceStatsUI(List<Piece> p1Pieces, List<Piece> p2Pieces)
    // {
    //     for (int i = 0; i < p1PieceStatUIs.Count; i++)
    //     {
    //         p1PieceStatUIs[i].Init(p1Pieces[i]);
    //         p2PieceStatUIs[i].Init(p2Pieces[i]);
    //     }
    // }

    public void TogglePieceUIPane(Guid pieceGuid)
    {
        GetPieceUIByGuid(pieceGuid).ToggleLargeStatsPane(true);
        currentOpenPieceDetails = pieceGuid;
    }
    public void CloseCurrentPieceDetails()
    {
        if (currentOpenPieceDetails != startGuid)
            GetPieceUIByGuid(currentOpenPieceDetails).ToggleLargeStatsPane(false);
    }

    public void TogglePieceUIGlow(Guid pieceGuid)
    {
        GetPieceUIByGuid(pieceGuid).ToggleGlow();
        currentGlow = pieceGuid;
    }

    public void TurnOffCurrentGlow()
    {
        if (currentGlow != startGuid)
            GetPieceUIByGuid(currentGlow).ToggleGlow(false);
    }

    public void UpdatePieceHealthBars(Board preBoard, Board postBoard)
    {
        Dictionary<Guid, Piece> preHealth = new Dictionary<Guid, Piece>();
        Dictionary<Guid, Piece> postHealth = new Dictionary<Guid, Piece>();

        // loop over both boards and track pre/post piece health
        BoardC.LoopTiles(preBoard.tiles, preTile =>
        {
            // add pre health
            if (preTile.Contents == TileContents.Piece)
                preHealth[preTile.Piece.Guid] = preTile.Piece;

            // add post health
            Tile postTile = BoardC.GetTile(postBoard, new Vector2(preTile.X, preTile.Y));
            if (postTile.Contents == TileContents.Piece)
                postHealth[postTile.Piece.Guid] = postTile.Piece;
        });

        // update health UI for each piece
        foreach (KeyValuePair<Guid, Piece> kvp in preHealth)
        {
            // check for any pieces that were destroyed and add a 0 post health for them
            if (!postHealth.ContainsKey(kvp.Key))
                postHealth[kvp.Key] = PieceC.UpdateHealth(kvp.Value, 0);

            Piece prePiece = preHealth[kvp.Key];
            Piece postPiece = postHealth[kvp.Key];

            // update health UI for each piece
            if (prePiece.Health != postPiece.Health)
                GetPieceUIByGuid(kvp.Key).UpdateHealthBar(postPiece.Health, prePiece.Health, postPiece.MaxHealth);
        }
    }

    public void UpdatePieceUIDeath(Guid guid)
    {
        GetPieceUIByGuid(guid).DeadAvatar();
    }

    // --- PieceSelectUI --- \\
    public void SelectPiece(string pieceLabel)
    {
        PieceLabel label = (PieceLabel)Enum.Parse(typeof(PieceLabel), pieceLabel);
        logic.SelectPiece(label, logic.localPlayer.PlayerToken);
    }

    public void TogglePieceSelect(bool isVisible)
    {
        pieceSelectUI.SetActive(isVisible);
    }

    // Calc (need to move to own script)
    public List<PieceStatsUI> GetPieceUIsByPlayer(PlayerToken player) => player == PlayerToken.P1 ? p1PieceStatUIs : p2PieceStatUIs;
    public PieceStatsUI GetPieceUIByGuid(Guid guid) => p1PieceStatUIs.Concat(p2PieceStatUIs).Where(ui => ui.guid == guid).First();
}
