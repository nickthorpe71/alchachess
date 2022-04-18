public class MoveData
{
    public readonly GenericPlayer player;
    public readonly TileData start;
    public readonly TileData end;

    public MoveData(GenericPlayer player, TileData start, TileData end)
    {
        this.player = player;
        this.start = start;
        this.end = end;
    }
}