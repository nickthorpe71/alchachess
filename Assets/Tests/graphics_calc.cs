using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class graphics_calc
{

    [Test]
    public void GetPieceByPosition_returns_correct_GameObject()
    {
        // arrange
        List<GameObject> pieces = new List<GameObject>{
            new GameObject(),
            new GameObject(),
            new GameObject(),
        };

        GameObject expected = pieces[1];
        expected.transform.position = new Vector3(1, 0, 2);

        // act 
        GameObject result = GraphicsC.GetPieceByPosition(pieces, new Vector2(1, 2));

        // assert
        Assert.AreEqual(expected, result);
    }
}
