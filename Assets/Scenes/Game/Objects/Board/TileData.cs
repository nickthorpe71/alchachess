using UnityEngine;

public class TileData
{
    private readonly ElementData elementData;
    private readonly PieceData pieceData;
    private readonly EnvironmentData activeEnvironment;
    private readonly Vector2 pos;

    public TileData(EnvironmentData activeEnvironment, Vector2 pos, ElementData element, PieceData piece)
    {
        this.activeEnvironment = activeEnvironment;
        this.pos = pos;
        this.elementData = element;
        this.pieceData = piece;
    }
}
