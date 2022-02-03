using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Data;

namespace Calc
{
    public static class BoardC
    {
        public static void LoopTiles(Tile[][] tiles, Action<Tile> f)
        {
            for (int y = 0; y < tiles.Length; y++)
                for (int x = 0; x < tiles[y].Length; x++)
                    f(tiles[y][x].Clone());
        }

        public static Tile[][] MapTiles(Tile[][] tiles, Func<Tile, Tile> f)
        {
            Tile[][] tilesCopy = new Tile[tiles.Length][];

            for (int y = 0; y < tiles.Length; y++)
            {
                tilesCopy[y] = new Tile[tiles[y].Length];
                for (int x = 0; x < tiles[y].Length; x++)
                    tilesCopy[y][x] = f(tiles[y][x].Clone());
            }

            return tilesCopy;
        }

        public static Tile[][] MapTilesBetween(Tile[][] tiles, Vector2 start, Vector2 end, Func<Tile, int, int, Tile> f)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => tile.Clone());

            Vector2 path = new Vector2(end.x, end.y) - new Vector2(start.x, start.y);
            int distance = Mathf.Max(Mathf.Abs((int)path.x), Mathf.Abs((int)path.y));
            Vector2 direction = path.normalized;
            direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

            for (int step = 0; step <= distance; step++)
            {
                int x = (int)start.x + ((int)direction.x * step);
                int y = (int)start.y + ((int)direction.y * step);
                Tile nextTile = tilesCopy[y][x];
                tilesCopy[y][x] = f(nextTile, x, y);
            }

            return tilesCopy;
        }

        public static Tile GetTile(Tile[][] tiles, int x, int y) => tiles[y][x];

        public static List<Tile> GetTilesWithPieceForPlayer(Tile[][] tiles, PlayerToken player)
        {
            List<Tile> result = new List<Tile>();
            LoopTiles(tiles, tile =>
            {
                if (tile.Contents == TileContents.Piece && tile.Piece.player == player)
                    result.Add(tile);
            });
            return result;
        }

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState, List<Vector2> toChange)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => tile.Clone());

            foreach (Vector2 pos in toChange)
            {
                Tile tileCopy = tilesCopy[(int)pos.y][(int)pos.x].Clone(newState, states);
                tilesCopy[(int)pos.y][(int)pos.x] = tileCopy;
            }

            return tilesCopy;
        }

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState)
            => MapTiles(tiles, (tile) =>
                {
                    Tile tileCopy = tile.Clone(newState, states);
                    return tileCopy;
                });

        public static List<Vector2> CalculateAOEPatterns(List<Vector2> pattern, Tile tile, PlayerToken player)
        {
            List<Vector2> result = new List<Vector2>();
            float playerMod = (player == PlayerToken.P1) ? 1 : -1;

            for (int i = 0; i < pattern.Count; i++)
            {
                Vector2 toAdd = new Vector2((float)(tile.X + pattern[i].x * playerMod), (float)(tile.Y + pattern[i].y * playerMod));
                if (InBounds(toAdd))
                    result.Add(toAdd);
            }

            return result;
        }

        public static List<Vector2> PossibleMoves(Tile[][] tiles, Tile selectedTile)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            int startX = selectedTile.X;
            int startY = selectedTile.Y;
            int moveDistance = selectedTile.Piece.moveDistance;

            List<int> activeDirections = GeneralC.CreateList(0, 1, 2, 3, 4, 5, 6, 7);

            for (int layer = 1; layer <= moveDistance; layer++)
            {
                List<Vector2> directions = new List<Vector2>(){
                    new Vector2(startX, startY + layer),
                    new Vector2(startX + layer, startY + layer),
                    new Vector2(startX + layer, startY),
                    new Vector2(startX + layer, startY - layer),
                    new Vector2(startX, startY - layer),
                    new Vector2(startX - layer, startY - layer),
                    new Vector2(startX - layer, startY),
                    new Vector2(startX - layer, startY + layer)
                };

                // remove any active directions that have a piece
                for (int i = 0; i < directions.Count; i++)
                    if (!CanTraverse(tiles, directions[i]) && activeDirections.Contains(i))
                        activeDirections.Remove(i);

                // filter directions to only include active directions
                directions = directions.Where((direction, index) => activeDirections.Contains(index)).ToList();

                possibleMoves.AddRange(directions);
            }
            return possibleMoves;
        }

        public static bool CanTraverse(Tile[][] tiles, Vector2 location)
        {
            if (!InBounds(location))
                return false;
            Tile tile = GetTile(tiles, (int)location.x, (int)location.y);
            if (tile.Contents == TileContents.Piece)
                return false;
            if (tile.Contents == TileContents.Environment)
                return false;

            return true;
        }

        public static bool InBounds(Vector2 location)
            => (location.x < Const.BOARD_WIDTH) && (location.x >= 0) && (location.y < Const.BOARD_HEIGHT) && (location.y >= 0);


        public static string GetRecipeByPath(Tile pathStart, Tile pathEnd, Tile[][] tiles, PlayerToken humanPlayer, PlayerToken currentPlayer)
        {
            if (currentPlayer == humanPlayer && !pathEnd.IsHighlighted)
                return "";

            string result = "";

            MapTilesBetween(tiles, new Vector2(pathStart.X, pathStart.Y), new Vector2(pathEnd.X, pathEnd.Y), (tile, x, y) =>
            {
                if (currentPlayer == humanPlayer && !tile.IsHighlighted) return tile;

                if (tile.Element != "N" && tile.Contents == TileContents.Element)
                    result += tile.Element;
                return tile;
            });

            return result;
        }

        public static Dictionary<Vector2, Tile> GetTilesWithPiecesInRange(Tile[][] tiles, List<Vector2> range)
        {
            Dictionary<Vector2, Tile> result = new Dictionary<Vector2, Tile>();
            LoopTiles(tiles, (tile) =>
            {
                Vector2 tilePosition = new Vector2(tile.X, tile.Y);
                if (range.Contains(tilePosition) && tile.Contents == TileContents.Piece)
                    result[tilePosition] = tile;
            });
            return result;
        }

        public static bool TileInRange(Tile tile, List<Vector2> range) => range.Contains(new Vector2(tile.X, tile.Y));

        public static string GetBoardAsString(Board board)
        {
            string result = "";
            for (int y = 0; y < board.tiles.Length; y++)
            {
                result += "\n";
                for (int x = 0; x < board.tiles[y].Length; x++)
                {
                    Tile tile = board.tiles[y][x];
                    string piece = tile.Piece != null ? tile.Piece.label.ToString() : "no piece";
                    result += "|" + tile.Element + "," + tile.Contents + "," + piece + "x:" + tile.X + "y:" + tile.Y + "|";
                }
            }

            return result;
        }

        public static Tile[][] RepopulateElements(Tile[][] tiles, Dictionary<Vector2, string> toRepopulate)
            => BoardC.MapTiles(tiles, (tile) =>
            {
                if (tile.Contents == TileContents.Empty && tile.Element != "N")
                {
                    // switch contents to Element
                    Tile tileCopy = tile.Clone(TileContents.Element);

                    // add element and position to dict for graphics
                    toRepopulate[new Vector2(tileCopy.X, tileCopy.Y)] = tile.Element;

                    return tileCopy;
                }
                return tile;
            });
    }
}