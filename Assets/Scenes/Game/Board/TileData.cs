using UnityEngine;

public class TileData
{
    public readonly ElementData elementData;
    public readonly PieceData pieceData;
    public readonly EnvironmentData activeEnvironment;
    public readonly Vector2 pos;

    public TileData(EnvironmentData activeEnvironment, Vector2 pos, ElementData element, PieceData piece)
    {
        this.activeEnvironment = activeEnvironment;
        this.pos = pos;
        this.elementData = element;
        this.pieceData = piece;
    }
}
