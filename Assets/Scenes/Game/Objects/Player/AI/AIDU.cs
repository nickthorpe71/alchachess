using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// ADU = AI Data Utilities
/// Abrievated because it's used frequently in AICalculation
/// </summary>
public static class AIDU
{
    // BOARD
    public static TileData GetTile(BoardData board, Vector2 pos) => board.tileData[(int)pos.y][(int)pos.x];
    public static void ForEachTile(BoardData board, Action<TileData> action)
    {
        for (int y = 0; y < board.height; y++)
            for (int x = 0; x < board.width; x++)
                action(board.tileData[y][x]);
    }

    public static bool IsInBounds(BoardData board, Vector2 pos)
    {
        bool inRow = pos.x < board.width && pos.x >= 0;
        bool inColumn = pos.y < board.height && pos.y >= 0;
        return inRow && inColumn;
    }
    public static List<TileData> GetTilePiecesForPlayer(BoardData board, GenericPlayer player)
    {
        List<TileData> tilesWithPieces = new List<TileData>();
        ForEachTile(board, tile =>
        {
            if (BelongsToPlayer(tile.pieceData, player))
                tilesWithPieces.Add(tile);
        });

        return tilesWithPieces;
    }
    public static List<Vector2> ValidateSpellPattern(BoardData board, List<Vector2> spellPattern, Vector2 pos)
    {
        return spellPattern
            .Select(target => new Vector2(target.x + pos.x, target.y + pos.y))
            .Where(target => IsInBounds(board, target))
            .ToList();
    }
    public static BoardData ApplyEnvironmentsToBoard(
        BoardData oldBoard,
        List<TileData> hitTiles,
        ElementData element)
    {

        BoardData newBoard = oldBoard.Clone();
        foreach (TileData tile in hitTiles)
        {
            newBoard.tileData[(int)tile.pos.y][(int)tile.pos.x] = new TileData(
                GetAlteredEnvironment(tile, element),
                tile.pos,
                tile.elementData,
                tile.pieceData
            );
        }
        return newBoard;
    }
    public static BoardData MovePiece(BoardData oldBoard, TileData oldPos, TileData newPos)
    {
        BoardData newBoard = oldBoard.Clone();

        newBoard.tileData[(int)oldPos.pos.y][(int)oldPos.pos.x] = new TileData(
            oldPos.activeEnvironment,
            oldPos.pos,
            oldPos.elementData,
            null
        );

        newBoard.tileData[(int)newPos.pos.y][(int)newPos.pos.x] = new TileData(
            newPos.activeEnvironment,
            newPos.pos,
            newPos.elementData,
            oldPos.pieceData
        );

        return newBoard;
    }

    // TILE
    public static bool CanTraverse(TileData tile) => tile.pieceData == null;

    // TODO: make this less gross of a branching tree
    public static EnvironmentData GetAlteredEnvironment(TileData target, ElementData element)
    {
        EnvironmentData environment = null;

        if (target.activeEnvironment == null)
        {
            switch (element.color)
            {
                case "Red":
                    environment = new FireEnv();
                    break;
                case "Blue":
                    environment = new WaterEnv();
                    break;
                case "Green":
                    environment = new PlantEnv();
                    break;
                case "Yellow":
                    environment = new RockEnv();
                    break;
            }
            return environment;
        }

        switch (target.activeEnvironment.GetType().Name)
        {
            case "FireEnv":
                switch (element.color)
                {
                    case "Red":
                        environment = target.activeEnvironment;
                        break;
                    case "Blue":
                        environment = null;
                        break;
                    case "Green":
                        environment = target.activeEnvironment;
                        break;
                    case "Yellow":
                        environment = new RockEnv();
                        break;
                }
                break;
            case "RockEnv":
                switch (element.color)
                {
                    case "Red":
                        environment = target.activeEnvironment;
                        break;
                    case "Blue":
                        environment = new WaterEnv();
                        break;
                    case "Green":
                        environment = null;
                        break;
                    case "Yellow":
                        environment = target.activeEnvironment;
                        break;
                }
                break;
            case "PlantEnv":
                switch (element.color)
                {
                    case "Red":
                        environment = new FireEnv();
                        break;
                    case "Blue":
                        environment = target.activeEnvironment;
                        break;
                    case "Green":
                        environment = target.activeEnvironment;
                        break;
                    case "Yellow":
                        environment = null;
                        break;
                }
                break;
            case "WaterEnv":
                switch (element.color)
                {
                    case "Red":
                        environment = null;
                        break;
                    case "Blue":
                        environment = target.activeEnvironment;
                        break;
                    case "Green":
                        environment = new PlantEnv();
                        break;
                    case "Yellow":
                        environment = target.activeEnvironment;
                        break;
                }
                break;
        }

        return environment;
    }

    // PIECE
    public static bool BelongsToPlayer(PieceData piece, GenericPlayer player)
        => piece != null && (piece.isGold == player.isGoldSide);

    public static List<Vector2> PossibleMoves(BoardData board, TileData start)
    {
        PieceData piece = start.pieceData;
        List<Vector2> possibleMoves = new List<Vector2>();
        IEnumerable<Vector2> patternWithStartAdjust = piece.movePattern
            .Select(move => new Vector2(start.pos.x + move.x, start.pos.y + move.y));

        for (int layer = 0; layer < piece.moveDistance; layer++)
        {
            // find all valid moves
            Vector2[] validMoves = patternWithStartAdjust
                .Select((dir, index) => new Vector2(
                    dir.x + layer * piece.movePattern[index].x,
                    dir.y + layer * piece.movePattern[index].y
                ))
                .Where((dir, index) =>
                {
                    bool inBounds = IsInBounds(board, dir);
                    bool canTraverse = false;
                    if (inBounds)
                        canTraverse = CanTraverse(GetTile(board, dir));
                    return inBounds && canTraverse;
                })
                .ToArray();

            possibleMoves.AddRange(validMoves);
        }

        return possibleMoves;
    }
}
