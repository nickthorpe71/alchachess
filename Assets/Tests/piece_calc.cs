using NUnit.Framework;
using System.Collections.Generic;
using Calc;
using Data;

public class piece_calc
{
    [Test]
    public void GetPathByLabel_gets_correct_path()
    {
        // arrange
        string expected = "Pieces/Esa";

        // act
        string result = PieceC.GetPathByLabel(PieceLabel.Esa);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CalcExpForNextLevel_gets_correct_exp_lvls_1_to_10()
    {
        // arrange
        Dictionary<int, int> levelToExpMapExpected = new Dictionary<int, int>{
            {1,56}, {2,1637}, {3,4743}, {4,10125}, {5,18681},
            {6,31462}, {7,49668}, {8,74650}, {9,107906}, {10,151087}
        };

        // act
        Dictionary<int, int> levelToExpMapRequired = new Dictionary<int, int>{
            {1,PieceC.ExpForNextLevel(1)}, {2,PieceC.ExpForNextLevel(2)}, {3,PieceC.ExpForNextLevel(3)}, {4,PieceC.ExpForNextLevel(4)}, {5,PieceC.ExpForNextLevel(5)}, {6,PieceC.ExpForNextLevel(6)}, {7,PieceC.ExpForNextLevel(7)}, {8,PieceC.ExpForNextLevel(8)}, {9,PieceC.ExpForNextLevel(9)}, {10, PieceC.ExpForNextLevel(10)}
        };

        // assert
        for (int i = 1; i <= 10; i++)
        {
            Assert.AreEqual(levelToExpMapExpected[i], levelToExpMapRequired[i]);
        }
    }

    [Test]
    public void ExpFromDefeatingOther_gets_correct_exp_lvls_1_to_2()
    {
        // act
        int result11 = PieceC.ExpFromDefeatingOther(1, 1); // lvl 1 defeating lvl 1
        int result21 = PieceC.ExpFromDefeatingOther(2, 1); // lvl 2 defeating lvl 1
        int result12 = PieceC.ExpFromDefeatingOther(1, 2); // lvl 1 defeating lvl 2
        int result22 = PieceC.ExpFromDefeatingOther(2, 2); // lvl 2 defeating lvl 2

        // assert
        Assert.AreEqual(45, result11);
        Assert.AreEqual(22, result21);
        Assert.AreEqual(1310, result12);
        Assert.AreEqual(655, result22);
    }
}
