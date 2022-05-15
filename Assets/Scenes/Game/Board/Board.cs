using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    Game game;
    private readonly int _width = 8;
    public int width { get { return _width; } }
    private readonly int _height = 8;
    public int height { get { return _height; } }
    private Tile[][] tiles;
    private List<Piece> pieces;
    private GameObject castAnim;
    private GameObject spellAnim;

    private void Awake()
    {
        game = GetComponent<Game>();
        pieces = new List<Piece>();
        ResetBoard(game);
        castAnim = Resources.Load("Piece/Anim/CastAnim") as GameObject;
        spellAnim = Resources.Load("Piece/Anim/SpellAnim") as GameObject;
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
    public void CastSpell(Tile tile, Piece caster)
    {
        StartCoroutine(CastRoutine(tile, caster));
    }
    IEnumerator CastRoutine(Tile tile, Piece caster)
    {
        yield return new WaitForSeconds(0.3f);

        List<Vector2> validatedSpellPattern = ValidateSpellPattern(tile.GetPattern(), tile.GetPos);

        foreach (Vector2 pos in validatedSpellPattern)
        {
            // play spell animation
            Instantiate(spellAnim, new Vector3(pos.x, 0.45f, pos.y), Quaternion.identity)

            // destroy pieces
            pieces.Remove(tile.GetPiece());
            tile.KillPiece();
        }
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
        foreach (Vector2 aoe in ValidateSpellPattern(GetTile(pos).GetPattern(), pos))
            GetTile(aoe).AOE(deactivate);
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

    public void ResetBoard(Game game)
    {
        TilePat[][] tilePatterns = new TilePat[][] {
            new TilePat[] {TilePat.X,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X},
            new TilePat[] {TilePat.X,TilePat.H,TilePat.H,TilePat.H,TilePat.H,TilePat.X,TilePat.X,TilePat.X},
        };

        PieceName[][] piecePattern = new PieceName[][] {
            new PieceName[] {PieceName.Kaido,PieceName.WhiteBeard,PieceName.Shanks,PieceName.Luffy,PieceName.Luffy,PieceName.Shanks,PieceName.WhiteBeard,PieceName.Kaido},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None,PieceName.None},
            new PieceName[] {PieceName.Kaido,PieceName.WhiteBeard,PieceName.Shanks,PieceName.Luffy,PieceName.Luffy,PieceName.Shanks,PieceName.WhiteBeard,PieceName.Kaido}
        };

        tiles = new Tile[height][];

        for (int y = 0; y < height; y++)
        {
            tiles[y] = new Tile[width];
            for (int x = 0; x < width; x++)
            {
                // instantiate tile
                GameObject tileObj = game.Spawn("Tile/Tile", new Vector3(x, 0, y), Quaternion.identity);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(new Vector2(x, y), tilePatterns[y][x]);
                tiles[y][x] = tile;

                // if there is a piece to instantiate
                if (piecePattern[y][x] != PieceName.None)
                {
                    bool isGold = true;
                    Quaternion rot = Quaternion.identity;

                    // if we are looking at the black side
                    if (y > height / 2 - 1)
                    {
                        isGold = false;
                        rot.y = 180;
                    }

                    GameObject pieceObj = game.Spawn($"Piece/{piecePattern[y][x]}", new Vector3(x, 0, y), rot);
                    Piece piece = pieceObj.GetComponent<Piece>();
                    piece.Init(isGold);
                    tile.SetPiece(piece);
                    pieces.Add(piece);
                }
            }
        }
    }
}

enum TilePat
{
    H, V, T, O, X
}
