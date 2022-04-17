using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    Game game;
    private readonly int _width = 6;
    public int width { get { return _width; } }
    private readonly int _height = 10;
    public int height { get { return _height; } }
    private Tile[][] tiles;
    private List<Piece> pieces;

    private void Awake()
    {
        game = GetComponent<Game>();
        pieces = new List<Piece>();
        ResetBoard(game);
    }

    public BoardData GetData()
    {
        TileData[][] tileData;
        tileData = new TileData[height][];
        for (int y = 0; y < height; y++)
        {
            tileData[y] = new TileData[width];
            for (int x = 0; x < width; x++)
            {
                tileData[y][x] = tiles[y][x].GetData();
            }
        }

        PieceData[] pieceData = pieces.Select(piece => piece.GetData()).ToArray();

        return new BoardData(tileData, pieceData, width, height);
    }

    public void CastSpell(Element element, Piece caster)
    {
        StartCoroutine(CastRoutine(element, caster));
    }
    IEnumerator CastRoutine(Element element, Piece caster)
    {
        Vector2 elementPos = new Vector2(element.transform.position.x, element.transform.position.z);
        List<Vector2> validatedSpellPattern = ValidateSpellPattern(element.GetSpellPattern(), elementPos);
        foreach (Vector2 pos in validatedSpellPattern)
        {
            Tile tile = GetTile(pos);

            // plan spell animation
            game.Spawn(element.spellAnimPath, new Vector3(pos.x, 0.45f, pos.y), Quaternion.identity);

            yield return new WaitForSeconds(0.1f);

            // change environment
            tile.ApplySpellToEnvironment(element.GetColor());

            if (tile.HasPiece() && !tile.IsImmuneToElement(element))
            {
                // apply spell/environmental effects
                if (element.HasKnockback())
                {
                    // determine direction
                    Vector2 direction = tile.GetPos() - elementPos;
                    // look at tile one more in that direction
                    Vector2 toCheck = tile.GetPos() + direction;

                    // if position past target tile is in bounds and can be traversed
                    if (IsInBounds(toCheck))
                    {
                        Tile nextTile = GetTile(toCheck);
                        if (nextTile.CanTraverse())
                            tile.TransferPiece(nextTile, warp: false);
                    }
                    else // if that is the edge of the board move piece and kill
                    {
                        tile.GetPiece().KnockOffBoard(direction);
                        pieces.Remove(tile.GetPiece());
                        tile.SetPiece(null);
                    }
                }
                else if (element.DestroysOccupant())
                {
                    pieces.Remove(tile.GetPiece());
                    tile.KillPiece();
                }
            }
        }

        if (!caster.isBeingKnockedBack)
            game.NextTurn();
        else
            caster.isBeingKnockedBack = false;
    }

    public void SetHighlightedMoves(Vector2 pos, bool deactivate = false)
    {
        Tile tile = GetTile(pos);
        if (!tile.HasPiece()) return;

        List<Vector2> possibleMoves = tile
            .GetPiece()
            .PossibleMoves(this);

        foreach (Vector2 move in possibleMoves)
            GetTile(move).Highlight(deactivate);
    }
    public void SetAOEMarkers(Vector2 pos, bool deactivate = false)
    {
        Tile hoveredTile = GetTile(pos);

        if (hoveredTile.HasActiveElement())
        {
            Element element = hoveredTile.GetElement();

            foreach (Vector2 aoe in ValidateSpellPattern(element.GetSpellPattern(), pos))
                GetTile(aoe).AOE(deactivate);
        }
    }

    public Tile GetTile(Vector2 pos) => tiles[(int)pos.y][(int)pos.x];

    public void LoopBoard(Action<Tile> action)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                action(tiles[y][x]);
    }

    public bool IsInBounds(Vector2 pos)
    {
        bool inRow = pos.x < width && pos.x >= 0;
        bool inColumn = pos.y < height && pos.y >= 0;
        return inRow && inColumn;
    }

    public List<Tile> GetTilePiecesForPlayer(GenericPlayer player)
    {
        List<Tile> tilesWithPieces = new List<Tile>();
        LoopBoard(tile =>
        {
            if (tile.HasPiece() && (tile.GetPiece().IsGold() == player.isGoldSide))
            {
                tilesWithPieces.Add(tile);
            }
        });

        return tilesWithPieces;
    }

    public List<Vector2> ValidateSpellPattern(List<Vector2> spellPattern, Vector2 pos)
    {
        return spellPattern
            .Select(target => new Vector2(target.x + pos.x, target.y + pos.y))
            .Where(target => IsInBounds(target)).ToList();
    }

    public void RepopulateElements()
    {
        LoopBoard(tile => tile.ActivateElement());
    }

    public void ResetBoard(Game game)
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

        tiles = new Tile[height][];

        for (int y = 0; y < height; y++)
        {
            tiles[y] = new Tile[width];
            for (int x = 0; x < width; x++)
            {
                // instantiate tile and element
                GameObject element = game.Spawn($"Element/{elementPattern[y][x]}", new Vector3(x, 0.45f, y), Quaternion.identity);
                element.GetComponent<Element>().Init(game, elementPattern[y][x]);
                GameObject tileObj = game.Spawn("Tile/Tile", new Vector3(x, 0, y), Quaternion.identity);
                element.transform.parent = tileObj.transform;
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(element, new Vector2(x, y));
                tiles[y][x] = tile;

                // if there is a piece to instantiate
                if (piecePattern[y][x] != "None")
                {
                    element.GetComponent<Element>().Deactivate(playAnim: false);
                    bool isGold = true;
                    Quaternion rot = Quaternion.identity;
                    string side = "Gold";

                    if (y > height / 2 - 1) // if we are looking at the black side
                    {
                        isGold = false;
                        rot.y = 180;
                        side = "Black";
                    }

                    GameObject pieceObj = game.Spawn($"Piece/{side}/{piecePattern[y][x]}", new Vector3(x, 0, y), rot);
                    Piece piece = pieceObj.GetComponent<Piece>();
                    piece.Init(isGold);
                    tile.SetPiece(piece);
                    pieces.Add(piece);
                }
            }
        }
    }
}
