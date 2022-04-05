public class Move
{
    public GenericPlayer player { get; }
    public Tile start { get; }
    public Tile end { get; }

    public Move(GenericPlayer player, Tile start, Tile end)
    {
        this.player = player;
        this.start = start;
        this.end = end;
    }
}