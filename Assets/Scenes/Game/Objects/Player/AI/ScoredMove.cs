public class ScoredMove : MoveData
{
    public int score { get; private set; }

    public ScoredMove(GenericPlayer player, TileData start, TileData end, int score) : base(player, start, end)
    {
        this.score = score;
    }
}