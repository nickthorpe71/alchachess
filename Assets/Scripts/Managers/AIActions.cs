using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Calc;

namespace Actions
{
    public static class AI
    {
        public static void ChoosePiece(GameLogic logic, List<PieceLabel> choices)
        {
            // temp to test only selecting death god
            if (logic.turnCount != 6 && logic.turnCount != 8)
            {
                int pickDemi = (int)Mathf.Floor(logic.turnCount / 2) - 1;
                if (logic.turnCount > 8)
                    pickDemi -= 2;
                logic.SelectPiece((PieceLabel)pickDemi, logic.currentPlayer);
                return;
            }

            // pick randomly for now
            int pick = Random.Range(0, choices.Count);
            logic.SelectPiece(choices[pick], logic.currentPlayer);
        }

        public static void TakeTurn(Board board, GameLogic logic)
        {
            // RandomTurn(board, logic);
            DamagingTurn(board, logic);
            // MiniMax(board, logic, 10);
        }

        private static void RandomTurn(Board board, GameLogic logic)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.remotePlayer.PlayerToken);

            // pick a random piece (start)
            Tile selectedPiece = GeneralC.RandomFromList(aiPieces);

            // determine possible moves
            List<Vector2> possibleMoves = BoardC.PossibleMoves(board.tiles, selectedPiece);

            // select random space from possible moves (end)
            Vector2 selectedMove = GeneralC.RandomFromList(possibleMoves);
            Tile endTile = board.tiles[(int)selectedMove.y][(int)selectedMove.x];

            // calculate spell from selected start and end of path
            Spell spell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(board, new Vector2(selectedPiece.X, selectedPiece.Y), new Vector2(endTile.X, endTile.Y)));

            logic.ExecuteMove(selectedPiece, endTile);
        }

        private static void DamagingTurn(Board board, GameLogic logic)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.remotePlayer.PlayerToken).ToList();

            // score possible moves
            List<ScoredMove> scoredMoves = AIC.GetScoredMoves(board, logic, aiPieces);

            ScoredMove bestMove = scoredMoves.OrderByDescending(move => move.Score).ToArray()[0];
            logic.ExecuteMove(bestMove.Start, bestMove.End);
        }

        private static void MiniMax(Board board, GameLogic logic, int depth)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.remotePlayer.PlayerToken).ToList();

            ScoredMove bestMove = AIC.MiniMaxEx(board, logic, aiPieces, depth);
            logic.ExecuteMove(bestMove.Start, bestMove.End);
        }
    }
}
