using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Calc;

namespace Actions
{
    public static class AI
    {
        // Actions
        public static void TakeTurn(Board board, GameLogic logic)
        {
            // RandomTurn(board, logic);
            DamagingTurn(board, logic);
        }

        private static void RandomTurn(Board board, GameLogic logic)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.aiPlayer);

            // pick a random piece (start)
            Tile selectedPiece = GeneralC.RandomFromList(aiPieces);

            // determine possible moves
            List<Vector2> possibleMoves = BoardC.PossibleMoves(board.tiles, selectedPiece);

            // select random space from possible moves (end)
            Vector2 selectedMove = GeneralC.RandomFromList(possibleMoves);
            Tile endTile = board.tiles[(int)selectedMove.y][(int)selectedMove.x];

            // calculate spell from selected start and end of path
            Spell spell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(selectedPiece, endTile, board.tiles, logic.humanPlayer, logic.currentPlayer));

            logic.ExecuteMove(selectedPiece, endTile, spell);
        }

        private static void DamagingTurn(Board board, GameLogic logic)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.aiPlayer).ToList();

            // score possible moves
            List<ScoredMove> scoredMoves = AIC.GetScoredMoves(board, logic, aiPieces);


            ScoredMove bestMove = scoredMoves.OrderByDescending(move => move.Score).ToArray()[0];
            logic.ExecuteMove(bestMove.Start, bestMove.End, bestMove.Spell);
        }
    }
}
