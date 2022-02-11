using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using Data;
using Calc;

public class board_calc
{
    [Test]
    public void LoopTiles_hits_each_tile_on_board()
    {
        // arrange
        Board board = new Board();
        int tileCount = 0;
        int expectedTileCount = Const.BOARD_HEIGHT * Const.BOARD_WIDTH;

        // act
        BoardC.LoopTiles(board.tiles, _ =>
        {
            tileCount++;
        });

        // assert
        Assert.AreEqual(expectedTileCount, tileCount);
    }

    [Test]
    public void GetTile_returns_correct_tile()
    {
        // arrange
        Board board = new Board();

        // act
        Tile result = BoardC.GetTile(board, new Vector2(1, 0));

        // assert
        Assert.AreEqual(PieceLabel.Esa, result.Piece.label);
        Assert.AreEqual(PlayerToken.P1, result.Piece.player);
    }

    [Test]
    public void PossibleMoves_returns_currect_list_of_Vector2s()
    {
        // arrange
        Board board = new Board();
        Tile selectedTile = board.tiles[0][1];
        List<Vector2> expected = new List<Vector2>
        {
            new Vector2(1,1),
            new Vector2(2,1),
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,2),
            new Vector2(3,2),
            new Vector2(1,3),
            new Vector2(4,3),
            new Vector2(1,4),
            new Vector2(5,4)
        };

        // act
        List<Vector2> result = BoardC.PossibleMoves(board.tiles, selectedTile);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CanTraverse_returns_true()
    {
        // arrange
        Board board = new Board();
        Vector2 location = new Vector2(1, 2);

        // act
        bool result = BoardC.CanTraverse(board.tiles, location);

        // assert
        Assert.AreEqual(true, result);
    }

    [Test]
    public void CanTraverse_returns_false()
    {
        // arrange
        Board board = new Board();
        Vector2 location = new Vector2(2, 0);

        // act
        bool result = BoardC.CanTraverse(board.tiles, location);

        // assert
        Assert.AreEqual(false, result);
    }

    [Test]
    public void InBounds_returns_true()
    {
        // arrange
        Vector3 location = new Vector3(2, 0, 0);

        // act
        bool result = BoardC.InBounds(location);

        // assert
        Assert.AreEqual(true, result);
    }

    [Test]
    public void InBounds_returns_false()
    {
        // arrange
        Vector3 location = new Vector3(20, 0, 0);

        // act
        bool result = BoardC.InBounds(location);

        // assert
        Assert.AreEqual(false, result);
    }

    [Test]
    public void ChangeTilesState_changes_state_on_all_tiles()
    {
        // arrange
        Board board = new Board();
        List<TileState> states = new List<TileState>
        {
            TileState.isClicked,
            TileState.isHovered,
            TileState.isHighlighted,
            TileState.isAOE
        };

        int numStatesChecked = states.Count;
        var allStates = Enum.GetValues(typeof(TileState));

        // act
        board.tiles = BoardC.ChangeTilesState(board.tiles, states, true);

        // assert
        BoardC.LoopTiles(board.tiles, tile =>
        {
            Assert.AreEqual(true, tile.IsClicked);
            Assert.AreEqual(true, tile.IsHovered);
            Assert.AreEqual(true, tile.IsHighlighted);
            Assert.AreEqual(true, tile.IsAOE);
        });

        // make sure all states are checked
        Assert.AreEqual(numStatesChecked, allStates.Length);
    }

}
