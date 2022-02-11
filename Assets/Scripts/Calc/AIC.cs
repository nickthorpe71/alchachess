using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Calc
{
    public static class AIC
    {
        public static List<ScoredMove> GetScoredMoves(Board board, GameLogic logic, List<Tile> movablePieces)
        {
            List<ScoredMove> scoredMoves = new List<ScoredMove>();

            // for each piece calc possible moves and score them
            foreach (Tile piece in movablePieces)
            {
                // calc possible moves for this piece
                List<Vector2> possibleMoves = BoardC.PossibleMoves(board.tiles, piece);

                // gather score for each possible move
                foreach (Vector2 move in possibleMoves)
                {
                    Spell spell = SpellC.GetSpellByRecipe(
                        BoardC.GetRecipeByPath(
                        piece,
                        board.tiles[(int)move.y][(int)move.x],
                        board.tiles,
                        logic.humanPlayer,
                        logic.currentPlayer
                        )
                    );

                    scoredMoves.Add(GetScoreForMove(board, spell, piece, move));
                }
            }

            return scoredMoves;
        }

        private static ScoredMove GetScoreForMove(Board board, Spell spell, Tile startTile, Vector2 move)
        {
            float score = 0;
            int numDeadPieces = 0;
            Tile endTile = TileC.Clone(BoardC.GetTile(board, move));

            if (spell == null) return new ScoredMove(startTile, endTile, spell, score);

            // check number of pieces this spell hits
            List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, endTile, startTile.Piece.player);
            Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(
                board.tiles,
                aoeRange
            );

            // calculate score
            foreach (Tile target in targetsPreDmg.Values)
            {
                Piece targetPostSpell = PieceC.ApplySpellToPiece(startTile.Piece, target.Piece, spell);

                // determine if target was enemy or ally
                bool targetIsAlly = startTile.Piece.player == target.Piece.player;
                // score is difference in health after spell effect
                float scoreToAdd = target.Piece.health - targetPostSpell.health;
                // scoreToAdd will be a negative if it is a heal, therefore we should
                // add it to the score (lowering the score) if it affects an enemy
                score += !targetIsAlly ? scoreToAdd : scoreToAdd * -1;

                // keep track of all pieces which have health <= 0
                if (targetPostSpell.health <= 0)
                    numDeadPieces++;
            }

            // add multiplier to score for dead pieces
            score *= (1 + numDeadPieces * 0.25f);

            return new ScoredMove(startTile, endTile, spell, score);
        }

        public static ScoredMove MiniMaxEx(Board board, GameLogic logic, List<Tile> movablePieces, int depth)
        {
            // copy board
            Board dummyBoard = new Board();
            dummyBoard.tiles = BoardC.MapTiles(board.tiles, tile => TileC.Clone(tile));

            // to store final scores
            List<ScoredMove> finalScoredMoves = new List<ScoredMove>();

            // recurse 
            List<ScoredMove> scoredMoves = GetScoredMoves(dummyBoard, logic, movablePieces);
            ScoredMove bestMove = scoredMoves.OrderByDescending(move => move.Score).ToArray()[0];

            // TODO: 
            // make the best move on the board for this player
            // increase or decrease score depelding on player

            // return best move
            return finalScoredMoves.OrderByDescending(move => move.Score).ToArray()[0];
        }
    }
}

