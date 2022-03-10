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
                    f(TileC.Clone(tiles[y][x]));
        }

        public static void LoopTilesParalell(Tile[][] tiles1, Tile[][] tiles2, Action<Tile, Tile> f)
        {
            for (int y = 0; y < tiles1.Length; y++)
                for (int x = 0; x < tiles1[y].Length; x++)
                    f(TileC.Clone(tiles1[y][x]), TileC.Clone(tiles2[y][x]));
        }

        public static Tile[][] MapTiles(Tile[][] tiles, Func<Tile, Tile> f)
        {
            Tile[][] tilesCopy = new Tile[tiles.Length][];

            for (int y = 0; y < tiles.Length; y++)
            {
                tilesCopy[y] = new Tile[tiles[y].Length];
                for (int x = 0; x < tiles[y].Length; x++)
                    tilesCopy[y][x] = f(TileC.Clone(tiles[y][x]));
            }

            return tilesCopy;
        }

        public static Tile[][] MapTilesBetween(Tile[][] tiles, Vector2 start, Vector2 end, Func<Tile, int, int, Tile> f)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => TileC.Clone(tile));

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

        public static Tile GetTile(Board b, Vector2 v) => b.tiles[(int)v.y][(int)v.x];

        public static Board RandomizeBoardElements(Board board)
        {
            Board boardCopy = Clone(board);

            string[] elements = new string[] { "D", "W", "R", "B", "Y", "G" };
            string[][] pattern = new string[Const.BOARD_HEIGHT][];

            for (int y = 0; y < Const.BOARD_HEIGHT; y++)
            {
                pattern[y] = new string[Const.BOARD_WIDTH];

                if (y < Const.BOARD_HEIGHT / 2)
                    for (int x = 0; x < Const.BOARD_WIDTH; x++)
                        pattern[y][x] = elements[UnityEngine.Random.Range(0, elements.Length)];
                else
                {
                    int yAdjust = (Const.BOARD_HEIGHT / 2) - ((y - Const.BOARD_HEIGHT / 2) + 1);
                    for (int x = 0; x < Const.BOARD_WIDTH; x++)
                        pattern[y][x] = pattern[yAdjust][x];

                    Array.Reverse(pattern[y]);
                }
            }

            for (int y = 0; y < Const.BOARD_HEIGHT; y++)
                for (int x = 0; x < Const.BOARD_WIDTH; x++)
                {
                    boardCopy.tiles[y][x] = TileC.UpdateElement(boardCopy.tiles[y][x], pattern[y][x]);
                    if (boardCopy.tiles[y][x].Contents != TileContents.Piece)
                        boardCopy.tiles[y][x] = TileC.UpdateContents(boardCopy.tiles[y][x], TileContents.Element);
                }

            return boardCopy;
        }

        public static List<Tile> GetTilesWithPieceForPlayer(Tile[][] tiles, PlayerToken player)
        {
            List<Tile> result = new List<Tile>();
            LoopTiles(tiles, tile =>
            {
                if (tile.Contents == TileContents.Piece && tile.Piece.Player == player)
                    result.Add(tile);
            });
            return result;
        }

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState, List<Vector2> toChange)
        {
            Tile[][] tilesCopy = MapTiles(tiles, (tile) => TileC.Clone(tile));

            foreach (Vector2 pos in toChange)
            {
                Tile tileCopy = TileC.UpdateStates(tilesCopy[(int)pos.y][(int)pos.x], newState, states);
                tilesCopy[(int)pos.y][(int)pos.x] = tileCopy;
            }

            return tilesCopy;
        }

        public static Tile[][] ChangeTilesState(Tile[][] tiles, List<TileState> states, bool newState)
            => MapTiles(tiles, (tile) =>
                {
                    Tile tileCopy = TileC.UpdateStates(tile, newState, states);
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
            int moveDistance = selectedTile.Piece.MoveDistance;

            List<int> activeDirections = selectedTile.Piece.MovePattern.Select(direction => (int)direction).ToList();

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
            Tile tile = tiles[(int)location.y][(int)location.x];
            if (tile.Contents == TileContents.Piece)
                return false;
            if (tile.Contents == TileContents.Environment)
                return false;

            return true;
        }

        public static bool InBounds(Vector2 location)
            => (location.x < Const.BOARD_WIDTH) && (location.x >= 0) && (location.y < Const.BOARD_HEIGHT) && (location.y >= 0);

        public static string GetRecipeByPath(Board board, Vector2 start, Vector2 end)
        {
            string result = "";

            MapTilesBetween(board.tiles, start, end, (tile, x, y) =>
            {
                if (tile.Element != "N" && tile.Contents == TileContents.Element)
                    result += tile.Element;
                return tile;
            });

            return result;
        }

        public static List<Tile> GetTilesWithPiecesInRange(Board board, List<Vector2> range)
        {
            List<Tile> result = new List<Tile>();
            LoopTiles(board.tiles, (tile) =>
            {
                Vector2 tilePosition = new Vector2(tile.X, tile.Y);
                if (range.Contains(tilePosition) && tile.Contents == TileContents.Piece)
                    result.Add(tile);
            });
            return result;
        }

        public static List<Vector2> GetNonPieceTilesInRange(string color, Board board)
        {
            List<Vector2> nonPieceTilesInRange = new List<Vector2>();

            // if this spell alters the environment
            if (SpellEffects.list[color].AltersEnvironment)
                // store tiles with no piece in range
                BoardC.LoopTiles(board.tiles, tile =>
                {
                    if (tile.Contents != TileContents.Piece && tile.Contents != TileContents.Environment)
                        nonPieceTilesInRange.Add(new Vector2(tile.X, tile.Y));
                });

            return nonPieceTilesInRange;
        }

        public static List<Vector2> GetNewlyAddedEnvironments(Board preBoard, Board postBoard)
        {
            List<Vector2> newEnvs = new List<Vector2>();
            LoopTiles(preBoard.tiles, preTile =>
            {
                Vector2 pos = new Vector2(preTile.X, preTile.Y);
                Tile postTile = GetTile(postBoard, pos);
                if (preTile.Contents != TileContents.Environment && postTile.Contents == TileContents.Environment)
                    newEnvs.Add(pos);
            });
            return newEnvs;
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
                    string piece = tile.Piece != null ? tile.Piece.Label.ToString() : "no piece";
                    result += "|" + tile.Element + "," + tile.Contents + "," + piece + "x:" + tile.X + "y:" + tile.Y + "|";
                }
            }

            return result;
        }

        public static Tile[][] RepopulateElements(Tile[][] tiles)
            => BoardC.MapTiles(tiles, (tile)
            => (tile.Contents == TileContents.Empty && tile.Element != "N")
            ? TileC.UpdateContents(tile, TileContents.Element)
            : tile);

        public static Board Clone(Board board)
        {
            Board boardCopy = new Board();
            boardCopy.tiles = BoardC.MapTiles(board.tiles, tile => TileC.Clone(tile));
            return boardCopy;
        }

        public static List<Vector2> GetDeadPiecesByComparison(Board preBoard, Board postBoard)
        {
            List<Tile> prePieceTiles = new List<Tile>();
            List<Guid> postPieces = new List<Guid>();

            LoopTilesParalell(preBoard.tiles, postBoard.tiles, (preTile, postTile) =>
            {
                if (preTile.Contents == TileContents.Piece)
                    prePieceTiles.Add(preTile);
                if (postTile.Contents == TileContents.Piece)
                    postPieces.Add(postTile.Piece.Guid);
            });

            return prePieceTiles
            .Where(tile => !postPieces.Contains(tile.Piece.Guid))
            .Select(tile => new Vector2(tile.X, tile.Y))
            .ToList();
        }

        public static Tile[][] RemovePiecesWithZeroHealth(Board board) => MapTiles(board.tiles, tile =>
         (tile.Contents == TileContents.Piece && tile.Piece.Health < 1)
         ? TileC.RemovePiece(tile)
         : TileC.Clone(tile));

        public static List<Vector2> GetRemovedContents(Board preBoard, Board postBoard, TileContents contentsToCheck)
        {
            List<Vector2> removed = new List<Vector2>();
            LoopTilesParalell(preBoard.tiles, postBoard.tiles, (preTile, postTile) =>
            {
                if (preTile.Contents == contentsToCheck && preTile.Contents != postTile.Contents)
                    removed.Add(new Vector2(preTile.X, preTile.Y));
            });
            return removed;
        }

        // ---- Board Turn Events ----

        public static MoveData ExecuteMove(Board board, PlayerToken currentPlayer, Vector2 start, Vector2 end)
        {
            Board boardPreMove = Clone(board);
            Board boardPostMove = Clone(board);

            // Move Phase
            boardPostMove = MovePiece(board, start, end);

            // Cast Phase
            Spell spell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(boardPreMove, start, end));
            if (spell != null)
                boardPostMove = CastSpell(boardPostMove, end, spell);

            // Upkeep
            boardPostMove = Upkeep(boardPostMove);

            return new MoveData(
                currentPlayer,
                GetTile(board, end).Piece,
                start,
                end,
                boardPreMove,
                boardPostMove,
                spell
            );
        }

        private static Board MovePiece(Board board, Vector2 start, Vector2 end)
        {
            Piece movingPiece = PieceC.Clone(GetTile(board, start).Piece);

            // move start piece to end tile
            board.tiles[(int)end.y][(int)end.x] = TileC.UpdatePiece(GetTile(board, end), movingPiece);

            // remove elements the piece moves over
            board.tiles = MapTilesBetween(board.tiles, start, end, (tile, x, y) =>
            {
                if (tile.Element != "N" && tile.Contents == TileContents.Element)
                    return TileC.UpdateContents(tile, TileContents.Empty);
                return tile;
            });

            // remove start piece from start tile
            board.tiles[(int)start.y][(int)start.x] = TileC.RemovePiece(GetTile(board, start));

            return board;
        }

        private static Board CastSpell(Board board, Vector2 casterPos, Spell spell)
        {
            Tile caster = GetTile(board, casterPos);

            // get aoe pattern
            List<Vector2> aoeRange = CalculateAOEPatterns(spell.Pattern, caster, caster.Piece.Player);
            // get tiles affected that don't have pieces
            List<Vector2> nonPieceTilesInRange = new List<Vector2>();
            // get tiles affected with pieces
            List<Tile> targetsPreDmg = GetTilesWithPiecesInRange(board, aoeRange);

            // apply damage/healing to pieces
            foreach (Tile tile in targetsPreDmg)
            {
                Piece piecePostSpell = PieceC.ApplySpellToPiece(caster.Piece, tile.Piece, spell);
                board.tiles[(int)tile.Y][(int)tile.X] = TileC.UpdatePiece(board.tiles[(int)tile.Y][(int)tile.X], piecePostSpell);
            };


            // if this spell alters the environment 
            if (SpellEffects.list[spell.Color].AltersEnvironment)
            {
                // store tiles with no piece in range
                board.tiles = BoardC.MapTiles(board.tiles, tile =>
                {
                    if (!BoardC.TileInRange(tile, aoeRange)) return tile;

                    // and save positions to place environment pieces to send to graphics
                    Tile tileCopy = TileC.Clone(tile);
                    if (tileCopy.Contents != TileContents.Piece && tileCopy.Contents != TileContents.Environment)
                    {
                        nonPieceTilesInRange.Add(new Vector2(tile.X, tile.Y));
                        tileCopy = TileC.UpdateRemainingEnvTime(tileCopy, SpellEffects.list[spell.Color].Duration);
                        tileCopy = TileC.UpdateContents(tileCopy, TileContents.Environment);
                    }
                    return tileCopy;
                });
            }

            return board;
        }

        public static Board Upkeep(Board board)
        {
            // remove dead pieces
            board.tiles = RemovePiecesWithZeroHealth(board);

            // upkeep environment effects
            board.tiles = BoardC.MapTiles(board.tiles, tile =>
            {
                if (tile.Contents != TileContents.Environment) return tile;
                // reduce count on environmet effects
                Tile tileCopy = TileC.UpdateRemainingEnvTime(tile, tile.RemainingTimeOnEnvironment - 1);

                // remove expired environmet effects from the board
                if (tileCopy.RemainingTimeOnEnvironment == 0)
                    tileCopy = TileC.UpdateContents(tileCopy, TileContents.Empty);

                return tileCopy;
            });

            // restore all elements to the field
            board.tiles = BoardC.RepopulateElements(board.tiles);
            return board;
        }
    }
}