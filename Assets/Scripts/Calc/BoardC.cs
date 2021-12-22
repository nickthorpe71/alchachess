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
                    f(tiles[y][x]);
        }

        public static Tile[][] MapTiles(Tile[][] tiles, Func<Tile, Tile> f)
        {
            Tile[][] tilesCopy = new Tile[tiles.Length][];

            for (int y = 0; y < tiles.Length; y++)
            {
                tilesCopy[y] = new Tile[tiles[y].Length];
                for (int x = 0; x < tiles[y].Length; x++)
                    tilesCopy[y][x] = f(tiles[y][x]);
            }

            return tilesCopy;
        }

        public static Tile GetTileDataByPos(Vector3 tilePos, Board board)
        => BoardC.GetTile(board.tiles, (int)tilePos.x, (int)tilePos.z);

        public static Tile GetTile(Tile[][] tiles, int x, int y) => tiles[y][x];

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState, List<Vector2> toChange)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => tile.Clone());

            for (int i = 0; i < toChange.Count; i++)
            {
                if (states.Contains(TileState.isClicked))
                    tilesCopy[(int)toChange[i].y][(int)toChange[i].x].isClicked = newState;
                if (states.Contains(TileState.isHovered))
                    tilesCopy[(int)toChange[i].y][(int)toChange[i].x].isHovered = newState;
                if (states.Contains(TileState.isHighlighted))
                    tilesCopy[(int)toChange[i].y][(int)toChange[i].x].isHighlighted = newState;
                if (states.Contains(TileState.isAOE))
                    tilesCopy[(int)toChange[i].y][(int)toChange[i].x].isAOE = newState;
            }

            return tilesCopy;
        }

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState)
            => MapTiles(tiles, (tile) =>
                {
                    Tile tileCopy = tile.Clone();

                    if (states.Contains(TileState.isClicked))
                        tileCopy.isClicked = newState;
                    if (states.Contains(TileState.isHovered))
                        tileCopy.isHovered = newState;
                    if (states.Contains(TileState.isHighlighted))
                        tileCopy.isHighlighted = newState;
                    if (states.Contains(TileState.isAOE))
                        tileCopy.isAOE = newState;

                    return tileCopy;
                });

        public static List<Vector2> CalculateAOEPatterns(List<V2Import> pattern, Tile tile)
        {
            List<Vector2> result = new List<Vector2>();

            // TODO: only add if in bounds

            for (int i = 0; i < pattern.Count; i++)
            {
                Vector2 toAdd = new Vector2((float)(tile.x + pattern[i].x), (float)(tile.y + pattern[i].y));
                if (InBounds(toAdd))
                    result.Add(toAdd);
            }

            return result;
        }

        public static List<Vector2> PossibleMoves(Tile[][] tiles, Tile selectedTile)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            int startX = selectedTile.x;
            int startY = selectedTile.y;
            int moveDistance = selectedTile.piece.moveDistance;

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
            if (TileHasPiece(tiles, location))
                return false;

            return true;
        }

        public static bool InBounds(Vector2 locaiton)
            => (locaiton.x < Const.BOARD_WIDTH) && (locaiton.x >= 0) && (locaiton.y < Const.BOARD_HEIGHT) && (locaiton.y >= 0);

        public static bool TileHasPiece(Tile[][] tiles, Vector2 location)
            => tiles[(int)location.y][(int)location.x].contents == TileContents.Piece;

        public static string GetRecipeByPath(Tile pathStart, Tile pathEnd, Tile[][] tiles)
        {
            if (!pathEnd.isHighlighted)
                return "";

            Vector2 path = new Vector2(pathEnd.x, pathEnd.y) - new Vector2(pathStart.x, pathStart.y);
            int distance = Mathf.Max((int)path.x, (int)path.y);
            Vector2 direction = path.normalized;
            direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

            string result = "";

            for (int step = 1; step <= distance; step++)
            {
                int x = (int)pathStart.x + ((int)direction.x * step);
                int y = (int)pathStart.y + ((int)direction.y * step);
                Tile nextTile = tiles[y][x];

                if (nextTile.element != 'N' && nextTile.isHighlighted && nextTile.contents == TileContents.Element)
                    result += nextTile.element;
            }

            return result;
        }

        public static Tile[][] ChangeTileContents(Tile[][] tiles, Vector2 position, TileContents newContents)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => tile.Clone());
            tilesCopy[(int)position.y][(int)position.x].contents = newContents;
            return tilesCopy;
        }

        public static Tile[][] UpdatePieceDataOnTile(Tile[][] tiles, Vector2 position, TileContents newContents, Piece newPieceData)
        {
            Tile[][] tilesCopy = ChangeTileContents(tiles, position, newContents);
            tilesCopy[(int)position.y][(int)position.x].piece = newPieceData;
            return tilesCopy;
        }

        public static List<Tile> GetTilesWithPiecesInRange(Tile[][] tiles, List<Vector2> range)
        {
            List<Tile> result = new List<Tile>();
            LoopTiles(tiles, (tile) =>
            {
                if (range.Contains(new Vector2(tile.x, tile.y)) && tile.contents == TileContents.Piece)
                    result.Add(tile);
            });
            return result;
        }
    }
}