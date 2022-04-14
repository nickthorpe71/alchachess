using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class BoardCalculation
{
    public static Tile GetTile(BoardData boardData, Vector2 pos) => boardData.tiles[(int)pos.y][(int)pos.x];
    public static void LoopBoard(BoardData boardData, Action<Tile> action)
    {
        for (int y = 0; y < boardData.height; y++)
            for (int x = 0; x < boardData.width; x++)
                action(boardData.tiles[y][x]);
    }
    public static bool IsInBounds(BoardData boardData, Vector2 pos)
    {
        bool inRow = pos.x < boardData.width && pos.x >= 0;
        bool inColumn = pos.y < boardData.height && pos.y >= 0;
        return inRow && inColumn;
    }

    public static List<Tile> GetTilePiecesForPlayer(BoardData boardData, GenericPlayer player)
    {
        List<Tile> tilesWithPieces = new List<Tile>();
        BoardCalculation.LoopBoard(boardData, tile =>
        {
            if (tile.HasPiece() && (tile.GetPiece().isGold == player.isGoldSide))
            {
                tilesWithPieces.Add(tile);
            }
        });

        return tilesWithPieces;
    }

    public static List<Vector2> ValidateSpellPattern(BoardData boardData, List<Vector2> spellPattern, Vector2 pos)
    {
        return spellPattern
            .Select(target => new Vector2(target.x + pos.x, target.y + pos.y))
            .Where(target => IsInBounds(boardData, target)).ToList();
    }

    public static void RepopulateElements(BoardData boardData)
    {
        LoopBoard(boardData, tile => tile.ActivateElement());
    }

    public static void ResetBoard(BoardData boardData)
    {
        string[][] elementPattern = new string[][] {
            new string[] {"Black","Blue","Red","Red", "Blue","Black"},
            new string[] {"Green","White","Yellow","Yellow","White", "Green"},
            new string[] {"White","Yellow","Red", "Red","Yellow", "White"},
            new string[] {"Green","Blue","Black","Black","Blue", "Green"},
            new string[] {"Red","Blue", "Green", "Black","White", "Yellow"},
            new string[] {"Yellow","White","Black","Green","Blue", "Red"},
            new string[] {"Green","Blue","Black","Black","Blue", "Green"},
            new string[] {"White","Yellow","Red", "Red","Yellow", "White"},
            new string[] {"Green","White","Yellow","Yellow","White", "Green"},
            new string[] {"Black","Blue","Red","Red", "Blue","Black"},
        };

        string[][] piecePattern = new string[][] {
            new string[] {"Demon","Demon","Witch","Witch","Demon","Demon"},
            new string[] {"Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle"},
            new string[] {"Demon","Demon","Witch","Witch","Demon","Demon"}

        };

        boardData.tiles = new Tile[boardData.height][];

        for (int y = 0; y < boardData.height; y++)
        {
            boardData.tiles[y] = new Tile[boardData.width];
            for (int x = 0; x < boardData.width; x++)
            {
                // instantiate tile and element
                GameObject element = GameCalculation.Spawn($"Element/{elementPattern[y][x]}", new Vector3(x, 0.45f, y), Quaternion.identity);
                element.GetComponent<Element>().Init(boardData, elementPattern[y][x]);
                GameObject tileObj = GameCalculation.Spawn("Tile/Tile", new Vector3(x, 0, y), Quaternion.identity);
                element.transform.parent = tileObj.transform;
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(element, new Vector2(x, y));
                boardData.tiles[y][x] = tile;

                // if there is a piece to instantiate
                if (piecePattern[y][x] != "None")
                {
                    element.GetComponent<Element>().Deactivate(playAnim: false);
                    bool isGold = true;
                    Quaternion rot = Quaternion.identity;
                    string side = "Gold";

                    if (y > boardData.height / 2 - 1) // if we are looking at the black side
                    {
                        isGold = false;
                        rot.y = 180;
                        side = "Black";
                    }

                    GameObject pieceObj = GameCalculation.Spawn($"Piece/{side}/{piecePattern[y][x]}", new Vector3(x, 0, y), rot);
                    Piece piece = pieceObj.GetComponent<Piece>();
                    piece.Init(isGold);
                    tile.SetPiece(piece);
                    boardData.pieces.Add(piece);
                }
            }
        }
    }
}
