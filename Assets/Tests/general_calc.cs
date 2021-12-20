using NUnit.Framework;
using Data;
using Calc;

public class general_calc
{
    [Test]
    public void RemoveAt_removes_correct_item()
    {
        // arrange
        Board board = new Board();
        Tile testTile = board.tiles[0][0];

        // act
        testTile = BoardC.SetTileContents(testTile, TileContents.Piece);

        // assert
        Assert.AreEqual(TileContents.Piece, testTile.contents);
    }
}
