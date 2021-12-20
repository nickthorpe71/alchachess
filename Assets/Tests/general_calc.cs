using NUnit.Framework;
using System.Collections.Generic;
using Calc;

public class general_calc
{
    [Test]
    public void RemoveAt_removes_correct_item_from_array()
    {
        // arrange
        int[] sample = new int[3] { 1, 2, 3 };
        int[] expected = new int[2] { 1, 3 };

        // act
        int[] result = GeneralC.RemoveAt(sample, 1);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void RemoveAt_removes_first_item_from_array()
    {
        // arrange
        int[] sample = new int[3] { 1, 2, 3 };
        int[] expected = new int[2] { 2, 3 };

        // act
        int[] result = GeneralC.RemoveAt(sample, 0);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void RemoveAt_removes_last_item_from_array()
    {
        // arrange
        int[] sample = new int[3] { 1, 2, 3 };
        int[] expected = new int[2] { 1, 2 };

        // act
        int[] result = GeneralC.RemoveAt(sample, 2);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CreateList_creates_list_from_array()
    {
        // arrange
        int[] sample = new int[3] { 1, 2, 3 };
        List<int> expected = new List<int>() { 1, 2, 3 };

        // act
        List<int> result = GeneralC.CreateList(sample);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CreateList_returns_empty_list_from_empty_array()
    {
        // arrange
        int[] sample = new int[0];
        List<int> expected = new List<int>();

        // act
        List<int> result = GeneralC.CreateList(sample);

        // assert
        Assert.AreEqual(expected, result);
    }
}
