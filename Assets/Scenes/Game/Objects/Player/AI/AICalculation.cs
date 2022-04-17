using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class AICalculation
{
    public static ScoredMove BestMove(Game game, GenericPlayer aiPlayer)
    {
        List<Tile> tilesWithPieces = game.board.GetTilePiecesForPlayer(aiPlayer);
        List<ScoredMove> scoredMoves = new List<ScoredMove>();

        foreach (Tile tile in tilesWithPieces)
        {
            // get list of possible moves for each piece
            List<Tile> possibleMoves = tile.GetPiece()
                .PossibleMoves(game.board)
                .Select(v2 => game.board.GetTile(v2)).ToList();
            // play each move out and score it
            scoredMoves.AddRange(possibleMoves.Select(move => ScoreMove(aiPlayer, game.board, tile, move)));
        }

        // choose move of highest score
        return new ScoredMove(aiPlayer, tilesWithPieces[0].GetData(), tilesWithPieces[1].GetData(), 0);
    }

    //TODO:
    // will also need:
    // - a way to apply the spells effect to the board so we can check multiple moves into the future

    private static ScoredMove ScoreMove(GenericPlayer aiPlayer, Board board, Tile start, Tile end)
    {
        // 1. check if move will cast a spell
        if (end.HasActiveElement())
        {
            // 2. check what spell will be cast on move
            Element element = end.GetElement().GetComponent<Element>();

            // 3. check what pieces that spell will hit
            List<Vector2> validatedSpellPattern = board.ValidateSpellPattern(element.GetSpellPattern(), end.GetPos());

            // 4. check if those pieces will die or move on hit


            // 5. if die add to score
            // 6. if move check if moving will kill them or cast a new spell
            // 7. if it kills them add to score
            // 8. if that cast a new spell repeat 2 - 8
        }

        return new ScoredMove(aiPlayer, start.GetData(), end.GetData(), 0);
    }
}