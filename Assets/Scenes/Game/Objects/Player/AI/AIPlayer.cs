using System.Collections.Generic;
using UnityEngine;
public class AIPlayer : GenericPlayer
{
    public AIPlayer(bool goldSide, bool isLocalPlayer)
    {
        isGoldSide = goldSide;
        isHumanPlayer = false;
        this.isLocalPlayer = isLocalPlayer;
    }

    public override void TakeTurn(Game game)
    {
        List<Tile> tilesWithPieces = BoardCalculation.GetTilePiecesForPlayer(game.board.boardData, this);

        List<ScoredMove> scoredMoves = new List<ScoredMove>();

        foreach (Tile tile in tilesWithPieces)
        {
            // get list of possible moves for each piece


            Debug.Log(tile.GetPiece().gameObject.name);
        }




        // play each move out and score it

        // 1. check if move will cast a spell
        // 2. check what spell will be cast on move
        // 3. check what pieces that spell will hit
        // 4. check if those pieces will die or move on hit
        // 5. if die add to score
        // 6. if move check if moving will kill them or cast a new spell
        // 7. if it kills them add to score
        // 8. if that cast a new spell repeat 2 - 8

        // choose move of highest score
    }
}