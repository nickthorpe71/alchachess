using System;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label)
            => String.Format("Pieces/{0}", Enum.GetName(typeof(PieceLabel), label));

        public static int CalcExpForNextLevel(int currentLevel)
            => (int)Math.Floor(((Math.Pow(currentLevel, 4) + 10 * Math.Pow(currentLevel, 3) + 37 * Math.Pow(currentLevel, 2) + 57 * currentLevel - 96) / 16) * 100);

        public static int CalcExpFromDefeatingOther(int myLevel, int opponentLevel)
            => (int)Math.Floor(((Math.Pow(opponentLevel, 4) + 10 * Math.Pow(opponentLevel, 3) + 37 * Math.Pow(opponentLevel, 2) + 57 * opponentLevel - 96) / myLevel) * 5);
    }
}
