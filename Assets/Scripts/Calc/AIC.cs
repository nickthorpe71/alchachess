using UnityEngine;
using System.Collections.Generic;
using Data;

namespace Calc
{
    public static class AIC
    {
        public static void TakeTurn(Board board, GameLogic logic)
        {
            Debug.Log("MY TURN!");
            RandomTurn(board, logic);

            // TODO: 
            // - simply check if there is a move that will damage player 
            //   or buff your team and if so take it. otherwise random move
            // - wipe board tiles state because sometimes they are staying highlighted after ai turn
            // - need to be able to click through pieces and elements
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

            // for each piece determine which moves will result in damage or healing
            // for each piece
            //      check where they can move, once moved what spell will be cast, 
            //      if that spell will do damage or heal their own piece
            //      give a score to this move based on what it will achieve

            // scoring
            // damage or heal adds to the score directly
            // killing a piece = score * 1.25

            // save these moves: Tile with piece(start), end location, spell, score

            logic.ExecuteMove(selectedPiece, endTile, spell);
        }
    }
}

