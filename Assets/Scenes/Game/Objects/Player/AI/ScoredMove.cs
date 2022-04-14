public class ScoredMove : MoveData
{
    public int score { get; private set; }

    public ScoredMove(GenericPlayer player, Tile start, Tile end, int score) : base(player, start, end)
    {
        this.score = score;
    }
}