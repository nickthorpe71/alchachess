using System.Collections.Generic;
using Data;

namespace Calc
{
    public static class PlayerC
    {
        public static PlayerToken GetOppositePlayer(PlayerToken currentPlayer) => currentPlayer == PlayerToken.P1 ? PlayerToken.P2 : PlayerToken.P1;

        public static bool CanHumanInput(PlayerToken currentPlayer) => currentPlayer == PlayerToken.P1;

        public static PlayerToken RandomizeFirstTurn()
        {
            int roll = UnityEngine.Random.Range(0, 100);
            return (roll < 50) ? PlayerToken.P1 : PlayerToken.P2;
        }

        public static PlayerData GetPlayerDataByToken(PlayerToken target, PlayerData p1, PlayerData p2) => target == p1.PlayerToken ? p1 : p2;

        public static PlayerData Clone(PlayerData player)
        {
            return new PlayerData(player.PlayerToken, player.PieceColor, player.HasWon, player.IsAI, player.Pieces);
        }

        public static PlayerData AddPiece(PlayerData player, Piece piece)
        {
            List<Piece> piecesClone = new List<Piece>(player.Pieces);
            piecesClone.Add(piece);

            return new PlayerData(
                player.PlayerToken,
                player.PieceColor,
                player.HasWon,
                player.IsAI,
                piecesClone
            );
        }
    }
}