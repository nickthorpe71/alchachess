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
        List<Tile> tilesWithPieces = game.GetTilePiecesForPlayer(this);

        foreach (Tile tile in tilesWithPieces)
        {
            Debug.Log(tile.GetPiece().gameObject.name);
        }

        // get list of possible moves
        // play each move out and score it
        // make move of highest score
    }
}