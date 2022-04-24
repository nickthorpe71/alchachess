using System;

[Serializable]
public abstract class GenericPlayer
{
    public bool isGoldSide { get; protected set; }
    public bool isHumanPlayer { get; protected set; }
    public bool isLocalPlayer { get; protected set; }
    private string[] pieceList = new string[6];

    public abstract void TakeTurn(Game game);

    public GenericPlayer()
    {
        pieceList = DefaultPieceList();
    }

    public GenericPlayer(string[] pieces)
    {
        pieceList = pieces;
    }

    private string[] DefaultPieceList()
    {
        return new string[] { "Kaido", "Shanks", "Luffy", "WhiteBeard", "Shanks", "Kaido" };
    }
}
