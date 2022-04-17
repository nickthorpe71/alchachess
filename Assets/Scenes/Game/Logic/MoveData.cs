public class MoveData
{
    public GenericPlayer player { get; }
    public TileData start { get; }
    public TileData end { get; }

    public MoveData(GenericPlayer player, TileData start, TileData end)
    {
        this.player = player;
        this.start = start;
        this.end = end;
    }
}