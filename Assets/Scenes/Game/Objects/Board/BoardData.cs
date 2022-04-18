using UnityEngine;

public class BoardData : MonoBehaviour
{
    private readonly TileData[][] tileData;
    private readonly PieceData[] pieceData;
    private readonly int width;
    private readonly int height;

    public BoardData(TileData[][] tileData, PieceData[] pieceData, int width, int height)
    {
        this.tileData = tileData;
        this.pieceData = pieceData;
        this.width = width;
        this.height = height;
    }

}
