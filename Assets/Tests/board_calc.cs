using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
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
    public void MapTiles_can_change_all_tiles_to_red_element()
    {
        // arrange
        Board board = new Board();
        char expectedElement = 'R';

        // act
        board.tiles = BoardC.MapTiles(board.tiles, tile =>
        {
            Tile copy = tile.Clone();
            copy.element = expectedElement;
            return copy;
        });

        // assert
        for (int y = 0; y < board.tiles.Length; y++)
            for (int x = 0; x < board.tiles[y].Length; x++)
                Assert.AreEqual(expectedElement, board.tiles[y][x].element);
    }

    [Test]
    public void GetTileDataByPos_returns_correct_tile()
    {
        // arrange
        Board board = new Board();

        // act
        Tile result = BoardC.GetTileDataByPos(new UnityEngine.Vector3(1, 0, 0), board);

        // assert
        Assert.AreEqual(PieceLabel.Esa, result.piece.label);
        Assert.AreEqual(PlayerToken.P1, result.piece.player);
    }

    [Test]
    public void GetTile_returns_correct_tile()
    {
        // arrange
        Board board = new Board();

        // act
        Tile result = BoardC.GetTile(board.tiles, 1, 0);

        // assert
        Assert.AreEqual(PieceLabel.Esa, result.piece.label);
        Assert.AreEqual(PlayerToken.P1, result.piece.player);
    }

    [Test]
    public void AddHighlightData_returns_highlighted_tiles()
    {
        // arrange
        Board board = new Board();
        Tile selectedTile = new Tile(1, 0, PieceLabel.Esa, PieceColor.White, PlayerToken.P1);
        int numHighlightedTiles = 0;

        // act
        board.tiles = BoardC.AddHighlightData(board.tiles, selectedTile);
        for (int y = 0; y < board.tiles.Length; y++)
            for (int x = 0; x < board.tiles[y].Length; x++)
                if (board.tiles[y][x].isHighlighted)
                    numHighlightedTiles++;

        // assert
        Assert.AreEqual(12, numHighlightedTiles);
    }

    [Test]
    public void PossibleMoves_returns_currect_list_of_Vector3s()
    {
        // arrange
        Board board = new Board();
        Tile selectedTile = new Tile(1, 0, PieceLabel.Esa, PieceColor.White, PlayerToken.P1);
        List<Vector3> expected = new List<Vector3>
        {
            new Vector3(1,0,1),
            new Vector3(2,0,1),
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,2),
            new Vector3(3,0,2),
            new Vector3(1,0,3),
            new Vector3(4,0,3),
            new Vector3(1,0,4),
            new Vector3(5,0,4),
            new Vector3(1,0,5),
            new Vector3(6,0,5)

        };

        // act
        List<Vector3> result = BoardC.PossibleMoves(board.tiles, selectedTile);

        // assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CanTraverse_returns_true()
    {
        // arrange
        Board board = new Board();
        Vector3 location = new Vector3(1, 0, 2);

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
        Vector3 location = new Vector3(2, 0, 0);

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
    public void TileHasPiece_returns_true()
    {
        // arrange
        Board board = new Board();
        Vector3 location = new Vector3(1, 0, 0);

        // act
        bool result = BoardC.TileHasPiece(board.tiles, location);

        // assert
        Assert.AreEqual(true, result);
    }

    [Test]
    public void TileHasPiece_returns_false()
    {
        // arrange
        Board board = new Board();
        Vector3 location = new Vector3(1, 0, 1);

        // act
        bool result = BoardC.TileHasPiece(board.tiles, location);

        // assert
        Assert.AreEqual(false, result);
    }

    [Test]
    public void GetRecipeByPath_returns_a_valid_recipe()
    {
        // arrange
        Board board = new Board();
        board.tiles[0][1].isClicked = true;
        board.tiles[1][1].isHighlighted = true;
        board.tiles[2][1].isHighlighted = true;
        board.tiles[3][1].isHighlighted = true;
        board.tiles[3][1].isHovered = true;

        Tile pathStart = board.tiles[0][1];
        Tile pathEnd = board.tiles[3][1];

        // act
        string result = BoardC.GetRecipeByPath(pathStart, pathEnd, board.tiles);

        // assert
        Assert.AreEqual("GDR", result);
    }

    [Test]
    public void RemoveStateFromAllTiles_clears_state()
    {
        // arrange
        Board board = new Board();
        board.tiles[1][1].isHighlighted = true;
        board.tiles[1][6].isHovered = true;
        board.tiles[0][1].isClicked = true;
        board.tiles[2][2].isAOE = true;

        // act
        board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isHighlighted);
        board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isHovered);
        board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isClicked);
        board.tiles = BoardC.RemoveStateFromAllTiles(board.tiles, TileState.isAOE);


        // assert
        Assert.AreEqual(false, board.tiles[1][1].isHighlighted);
        Assert.AreEqual(false, board.tiles[1][6].isHovered);
        Assert.AreEqual(false, board.tiles[0][1].isClicked);
        Assert.AreEqual(false, board.tiles[2][2].isAOE);
    }

    [Test]
    public void SetTileState_sets_state()
    {
        // arrange
        Board board = new Board();

        Tile highlighted = board.tiles[1][1];
        Tile hovered = board.tiles[1][6];
        Tile clicked = board.tiles[0][1];
        Tile aoe = board.tiles[2][2];

        int numStatesChecked = 4;
        var allStates = Enum.GetValues(typeof(TileState));

        // act
        highlighted = BoardC.SetTileState(highlighted, TileState.isHighlighted, true);
        hovered = BoardC.SetTileState(hovered, TileState.isHovered, true);
        clicked = BoardC.SetTileState(clicked, TileState.isClicked, true);
        aoe = BoardC.SetTileState(aoe, TileState.isAOE, true);

        // assert
        Assert.AreEqual(true, highlighted.isHighlighted);
        Assert.AreEqual(true, hovered.isHovered);
        Assert.AreEqual(true, clicked.isClicked);
        Assert.AreEqual(true, aoe.isAOE);

        // make sure all states are checked
        Assert.AreEqual(numStatesChecked, allStates.Length);
    }

    [Test]
    public void UpdateTileStateOnBoard_sets_state()
    {
        // arrange
        Board board = new Board();

        // act
        board.tiles = BoardC.UpdateTileStateOnBoard(board.tiles, 1, 1, TileState.isHovered, true);

        // assert
        Assert.AreEqual(true, board.tiles[1][1].isHovered);
    }

    [Test]
    public void SetTileContents_sets_contents()
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
