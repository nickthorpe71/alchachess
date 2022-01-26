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
    public void MapTiles_can_change_all_tiles_to_red_element()
    {
        // arrange
        Board board = new Board();
        string expectedElement = "R";

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
        string result = BoardC.GetRecipeByPath(pathStart, pathEnd, board.tiles, PlayerToken.P1, PlayerToken.P1);

        // assert
        Assert.AreEqual("GDR", result);
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
            Assert.AreEqual(true, tile.isClicked);
            Assert.AreEqual(true, tile.isHovered);
            Assert.AreEqual(true, tile.isHighlighted);
            Assert.AreEqual(true, tile.isAOE);
        });

        // make sure all states are checked
        Assert.AreEqual(numStatesChecked, allStates.Length);
    }

    [Test]
    public void MapTilesBetween_moves_a_piece_up()
    {
        // arrange
        Board resultBoard = new Board();
        Piece piece = resultBoard.tiles[0][4].piece;
        Vector2 start = new Vector2(4, 0);
        Vector2 end = new Vector2(4, 2);

        // act
        resultBoard.tiles = BoardC.MapTilesBetween(resultBoard.tiles, start, end, (tile, x, y) =>
        {
            Vector2 pos = new Vector2(x, y);
            if (pos == end)
            {   // set end tile contents to piece and piece data to moved piece
                tile.contents = TileContents.Piece;
                tile.piece = piece;
            }
            else
            {   // for all other tiles set contents to empty and piece to null
                tile.contents = TileContents.Empty;
                tile.piece = null;
            }

            return tile;
        });

        // assert
        Assert.AreEqual(TileContents.Empty, resultBoard.tiles[0][4].contents);
        Assert.AreEqual(null, resultBoard.tiles[0][4].piece);
        Assert.AreEqual(TileContents.Empty, resultBoard.tiles[1][4].contents);
        Assert.AreEqual(null, resultBoard.tiles[1][4].piece);
        Assert.AreEqual(TileContents.Piece, resultBoard.tiles[2][4].contents);
        Assert.AreEqual(piece, resultBoard.tiles[2][4].piece);
    }

    [Test]
    public void MapTilesBetween_moves_a_piece_up_and_back()
    {
        // arrange
        Board resultBoard = new Board();
        Piece piece = resultBoard.tiles[0][4].piece;
        Vector2 start = new Vector2(4, 0);
        Vector2 end = new Vector2(4, 2);

        // act
        resultBoard.tiles = BoardC.MapTilesBetween(resultBoard.tiles, start, end, (tile, x, y) =>
        {
            Vector2 pos = new Vector2(x, y);
            if (pos == end)
            {   // set end tile contents to piece and piece data to moved piece
                tile.contents = TileContents.Piece;
                tile.piece = piece;
            }
            else
            {   // for all other tiles set contents to empty and piece to null
                tile.contents = TileContents.Empty;
                tile.piece = null;
            }

            return tile;
        });

        start = new Vector2(4, 2);
        end = new Vector2(4, 0);

        resultBoard.tiles = BoardC.MapTilesBetween(resultBoard.tiles, start, end, (tile, x, y) =>
        {
            Vector2 pos = new Vector2(x, y);
            if (pos == end)
            {   // set end tile contents to piece and piece data to moved piece
                tile.contents = TileContents.Piece;
                tile.piece = piece;
            }
            else
            {   // for all other tiles set contents to empty and piece to null
                tile.contents = TileContents.Empty;
                tile.piece = null;
            }

            return tile;
        });

        // assert
        Assert.AreEqual(TileContents.Piece, resultBoard.tiles[0][4].contents);
        Assert.AreEqual(piece, resultBoard.tiles[0][4].piece);
        Assert.AreEqual(TileContents.Empty, resultBoard.tiles[1][4].contents);
        Assert.AreEqual(null, resultBoard.tiles[1][4].piece);
        Assert.AreEqual(TileContents.Empty, resultBoard.tiles[2][4].contents);
        Assert.AreEqual(null, resultBoard.tiles[2][4].piece);
    }
}
