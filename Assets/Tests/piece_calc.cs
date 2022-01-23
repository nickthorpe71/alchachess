using NUnit.Framework;
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
}
