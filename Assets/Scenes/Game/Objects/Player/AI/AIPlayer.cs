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
        ScoredMove bestMove = AICalculation.BestMove(game, this);
        MoveData randomMove = AICalculation.RandomMove(game, this);
        game.SubmitMove(randomMove.start.pos, randomMove.end.pos);
    }
}