using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    private Tile[][] tiles;
    private List<Piece> pieces = new List<Piece>();
    private readonly int _width = 6;
    public int width { get { return _width; } }
    private readonly int _height = 6;
    public int height { get { return _height; } }


    public Tile GetTile(Vector2 pos) => tiles[(int)pos.y][(int)pos.x];

    public bool IsInBounds(Vector2 pos)
    {
        bool inRow = pos.x < width && pos.x >= 0;
        bool inColumn = pos.y < height && pos.y >= 0;
        return inRow && inColumn;
    }

    public void Init(LifeCycle lifeCycle)
    {
        string[][] elementPattern = new string[][] {
            new string[] {"Blue","Black","Red","White","Yellow","Green"},
            new string[] {"Yellow","White","Green","Red","Blue","Black"},
            new string[] {"Green","Blue","Yellow","Black","White","Red"},
            new string[] {"Red","White","Black","Yellow","Blue","Green"},
            new string[] {"Black","Blue","Red","Green","White","Yellow"},
            new string[] {"Green","Yellow","White","Red","Black","Blue"}
        };

        string[][] piecePattern = new string[][] {
            new string[] {"Demon","Witch","Wraith","GodOfLife","Witch","Demon"},
            new string[] {"Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"None","None","None","None","None","None"},
            new string[] {"Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle","Gargoyle"},
            new string[] {"Demon","Witch","GodOfLife","Wraith","Witch","Demon"}
        };

        tiles = new Tile[_height][];

        for (int y = 0; y < _height; y++)
        {
            tiles[y] = new Tile[_width];
            for (int x = 0; x < _width; x++)
            {
                GameObject element = lifeCycle.Spawn($"Element/{elementPattern[y][x]}", new Vector3(x, 0.3f, y), Quaternion.identity);
                GameObject tileObj = lifeCycle.Spawn("Tile/Tile", new Vector3(x, 0, y), Quaternion.identity);
                element.transform.parent = tileObj.transform;
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(element, new Vector2(x, y));
                tiles[y][x] = tile;

                if (piecePattern[y][x] == "None") continue;
                element.SetActive(false);
                bool isGold = true;
                Quaternion rot = Quaternion.identity;
                string side = "Gold";

                if (y > 1) // if we are looking at the black side
                {
                    isGold = false;
                    rot.y = 180;
                    side = "Black";
                }

                GameObject pieceObj = lifeCycle.Spawn($"Piece/{side}/{piecePattern[y][x]}", new Vector3(x, 0, y), rot);
                Piece piece = pieceObj.GetComponent<Piece>();
                piece.Init(isGold);
                tile.SetPiece(piece);
                pieces.Add(piece);
            }
        }
    }
}
