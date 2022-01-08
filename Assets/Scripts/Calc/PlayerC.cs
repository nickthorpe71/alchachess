using Data;

namespace Calc
{
    public static class PlayerC
    {
        public static PlayerToken SwitchPlayers(PlayerToken currentPlayer) => currentPlayer == PlayerToken.P1 ? PlayerToken.P2 : PlayerToken.P1;

        public static bool CanHumanInput(PlayerToken currentPlayer) => currentPlayer == PlayerToken.P1;

        public static PlayerToken RandomizeFirstTurn()
        {
            int roll = UnityEngine.Random.Range(0, 100);
            return (roll < 50) ? PlayerToken.P1 : PlayerToken.P2;
        }
    }
}