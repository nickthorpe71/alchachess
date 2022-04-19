using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AICalculation
{
    public static MoveData RandomMove(Game game, GenericPlayer aiPlayer)
    {
        var rnd = new System.Random();
        return game.board.GetTilePiecesForPlayer(aiPlayer)
            // get all pieces for AI
            .Select(tile => tile.GetPiece()
            // get all possible moves for those pieces
            .PossibleMoves(game.board)
            // create movedata for each possible move
            .Select(possibleMove => new MoveData
                (
                    aiPlayer,
                    tile.GetData(),
                    game.board.GetTile(possibleMove).GetData()
                )
            ))
            // merge all lists into one list
            .SelectMany(moveDataList => moveDataList)
            // order randomly
            .OrderBy(x => rnd.Next())
            // take the first item in the list
            .ToList()[0];
    }

    public static ScoredMove BestMove(Game game, GenericPlayer aiPlayer)
    {
        List<ScoredMove> scoredMoves = ScoreAllMoves(game.board.GetData().Clone(), aiPlayer);
        // TODO: need to handle edgecase where there are no moves to make
        // Should also consider adding in difficulty/should AI always pick the best move?

        return scoredMoves[0];
    }

    public static List<ScoredMove> ScoreAllMoves(BoardData board, GenericPlayer aiPlayer)
    {
        // get tiles with pieces for aiPlayer
        return AIDU.GetTilePiecesForPlayer(board, aiPlayer)
            // get a list of possible moves for each piece
            .Select(start => AIDU.PossibleMoves(board, start)
                // get the end tile of each move
                .Select(endTile => AIDU.GetTile(board, endTile))
                // play each move out and score it
                .Select(end => new ScoredMove(
                        aiPlayer,
                        start,
                        end,
                        ScoreMove(aiPlayer, board, start, end)
                    )
                )
        )
        // merge all lists into one list
        .SelectMany(scoredMoveList => scoredMoveList)
        .OrderByDescending(scoredMove => scoredMove.score)
        .ToList();
    }

    private static int ScoreMove(GenericPlayer aiPlayer, BoardData board, TileData start, TileData end)
    {
        // 1. if landed on a tile with no active element
        if (end.activeEnvironment != null) return 0;

        // 2. check what spell will be cast on move
        ElementData element = AIDU.GetTile(board, end.pos).elementData;

        // 3. get tiles that spell will hit
        List<TileData> hitTiles = AIDU
            .ValidateSpellPattern(board, element.spellPattern, end.pos)
            .Select(pos => AIDU.GetTile(board, pos))
            .ToList();

        // 4. get pieces that spell will hit
        List<TileData> hitTilesWithPieces = hitTiles
            .Where(tile => tile.pieceData != null && tile.pos != start.pos)
            .ToList();

        // 5. if no pieces affected return 0
        if (hitTilesWithPieces.Count == 0) return 0;

        // 6. check if spell will destroy or knockback pieces
        // 7. if destroy, kill pieces and add to score
        if (element.destroysOccupant)
        {
            // determine which pieces belong to AI
            List<PieceData> hitAIPieces = hitTilesWithPieces
                .Select(tile => AIDU.BelongsToPlayer(tile.pieceData, aiPlayer) ? tile.pieceData : null)
                .Where(piece => piece != null)
                .ToList();

            int deadAIPieces = hitAIPieces.Count;
            int deadOpponentPieces = hitTilesWithPieces.Count - deadAIPieces;
            return deadOpponentPieces - deadAIPieces;
        }

        // 8. apply environments to a clone of the board
        BoardData updatedEnvBoard = AIDU.ApplyEnvironmentsToBoard(board, hitTiles, element);

        // 9. else the spell is causing hit pieces to be knocked back
        // 10. determine where pieces will land
        //    and recursively play out resulting moves
        return hitTilesWithPieces.Select(tile =>
        {
            // 10.a determine pieces knockback direction from end
            Vector2 direction = tile.pos - end.pos;

            // 10.b if no tile then the pieces is knocked off, add to score
            if (!AIDU.IsInBounds(updatedEnvBoard, direction))
                return AIDU.BelongsToPlayer(tile.pieceData, aiPlayer) ? -1 : 1;

            // 10.c if new tile kills them add to score 
            if (tile.elementData.destroysOccupant)
                return AIDU.BelongsToPlayer(tile.pieceData, aiPlayer) ? -1 : 1;

            // 10.d move piece
            BoardData updatedPieceBoard = AIDU.MovePiece(updatedEnvBoard, tile, AIDU.GetTile(updatedEnvBoard, direction));

            // 10.e else get the tile in that direction
            TileData newPosTile = AIDU.GetTile(updatedEnvBoard, direction);

            // 10.f else repeat from step 1
            return ScoreMove(aiPlayer, updatedPieceBoard, tile, newPosTile);
        }).Sum();
    }
}