public class AIPlayer : GenericPlayer
{
    public AIPlayer(bool goldSide, bool isLocalPlayer)
    {
        isGoldSide = goldSide;
        isHumanPlayer = false;
        this.isLocalPlayer = isLocalPlayer;
    }

    public override void TakeTurn(Game game)
    {
        // ScoredMove bestMove = AICalculation.BestMove(game, this);
        // game.SubmitMove(bestMove.start.GetPos(), bestMove.end.pos);
    }
}