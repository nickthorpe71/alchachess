public class BoardData
{
    public readonly TileData[][] tileData;
    public readonly PieceData[] pieceData;
    public readonly int width;
    public readonly int height;

    public BoardData(TileData[][] tileData, PieceData[] pieceData, int width, int height)
    {
        this.tileData = tileData;
        this.pieceData = pieceData;
        this.width = width;
        this.height = height;
    }

    public BoardData Clone()
    {
        return new BoardData(
            tileData,
            pieceData,
            width,
            height
        );
    }

}
