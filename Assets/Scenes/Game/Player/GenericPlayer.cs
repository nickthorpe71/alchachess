public abstract class GenericPlayer
{
    public bool isGoldSide { get; protected set; }
    public bool isHumanPlayer { get; protected set; }
    public bool isLocalPlayer { get; protected set; }

    public abstract void TakeTurn(Game game);
}
