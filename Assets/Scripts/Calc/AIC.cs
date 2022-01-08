using UnityEngine;
using Data;

namespace Calc
{
    public static class AIC
    {
        public static void TakeTurn(Board board, GameLogic logic)
        {
            Debug.Log("MY TURN!");
            RandomTurn(board, logic);
        }

        private static void RandomTurn(Board board, GameLogic logic)
        {
            // pick a random piece (start)
            // determine possible moves
            // select random space from possible moves (end)
            // calculate spell from selected start and end of path
            // call logic.ExecuteMove(start, end, spell);
        }
    }
}

