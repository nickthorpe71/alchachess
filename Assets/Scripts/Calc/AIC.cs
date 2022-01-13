using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Calc
{
    public static class AIC
    {
        public static void TakeTurn(Board board, GameLogic logic, int difficulty)
        {
            // RandomTurn(board, logic);
            DamagingTurn(board, logic, difficulty);

            // TODO: 
            // - wipe board tiles state because sometimes they are staying highlighted after ai turn
            // - need to be able to click through pieces and elements
            // - need to flip opponent spell patterns as they are the same rotation as player right now
            // - need to add healing team consideration to ai move score
            // - need to display freezing and unfreezing anim
            // - simplify stats and exp algo and other algos so it's easier(still probably not that easy, but this is for advanced players) for player to calc on the fly
            // - add more elements to board?
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

        private static void DamagingTurn(Board board, GameLogic logic, int difficulty)
        {
            // get pieces owned by ai player
            List<Tile> aiPieces = BoardC.GetTilesWithPieceForPlayer(board.tiles, logic.aiPlayer).Where(tile => tile.piece.currentSpellEffect != "frozen").ToList();

            List<ScoredMove> scoredMoves = new List<ScoredMove>();

            // for each piece calc possible moves and score them
            foreach (Tile piece in aiPieces)
            {
                // calc possible moves for this piece
                List<Vector2> possibleMoves = BoardC.PossibleMoves(board.tiles, piece);

                // gather scores for each possible move
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    float score = 0;
                    int numDeadPieces = 0;

                    Spell spell = SpellC.GetSpellByRecipe(
                        BoardC.GetRecipeByPath(
                            piece,
                            board.tiles[(int)possibleMoves[i].y][(int)possibleMoves[i].x],
                            board.tiles,
                            logic.humanPlayer,
                            logic.currentPlayer
                        )
                    );

                    Tile endTile = board.tiles[(int)possibleMoves[i].y][(int)possibleMoves[i].x];

                    if (spell != null)
                    {
                        // check number of pieces this spell hits

                        List<Vector2> aoeRange = BoardC.CalculateAOEPatterns(spell.pattern, endTile, piece.piece.player);

                        Dictionary<Vector2, Tile> targetsPreDmg = BoardC.GetTilesWithPiecesInRange(
                            board.tiles,
                            aoeRange,
                            logic.humanPlayer
                        );

                        // calculate pieces health post damage
                        foreach (Tile target in targetsPreDmg.Values)
                        {

                            Piece piecePostSpell = PieceC.ApplySpellToPiece(piece.piece, target.piece, spell);
                            score += target.piece.health - piecePostSpell.health;

                            // keep track of all pieces which have health <= 0
                            if (piecePostSpell.health <= 0)
                                numDeadPieces++;
                        }

                        score *= (1 + numDeadPieces * 0.25f);
                    }

                    ScoredMove move = new ScoredMove(piece, endTile, spell, score);

                    scoredMoves.Add(move);
                }
            }

            ScoredMove bestMove = scoredMoves.OrderByDescending(move => move.Score).ToArray()[0 + 5 - difficulty];
            logic.ExecuteMove(bestMove.Start, bestMove.End, bestMove.Spell);
        }

        public class ScoredMove
        {
            private readonly Tile _start;
            private readonly Tile _end;
            private readonly Spell _spell;
            private readonly float _score;

            public ScoredMove(Tile start, Tile end, Spell spell, float score)
            {
                _start = start;
                _end = end;
                _spell = spell;
                _score = score;
            }

            public Tile Start { get { return _start; } }

            public Tile End { get { return _end; } }

            public Spell Spell { get { return _spell; } }

            public float Score { get { return _score; } }
        }
    }
}

