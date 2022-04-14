using System.Collections.Generic;

public class BoardData
{
    public Tile[][] tiles { get; set; }
    public List<Piece> pieces { get; set; }
    private readonly int _width = 6;
    public int width { get { return _width; } }
    private readonly int _height = 10;
    public int height { get { return _height; } }

    public BoardData()
    {
        pieces = new List<Piece>();
    }
}
