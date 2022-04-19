public class HumanPlayer : GenericPlayer
{
    public HumanPlayer(bool goldSide, bool isLocalPlayer)
    {
        isGoldSide = goldSide;
        isHumanPlayer = true;
        this.isLocalPlayer = isLocalPlayer;
    }

    public override void TakeTurn(Game game)
    {
        return;
    }
}
