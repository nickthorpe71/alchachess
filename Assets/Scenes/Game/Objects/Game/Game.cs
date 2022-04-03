using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private GenericPlayer[] _players;
    public Board board { get; private set; }
    public GenericPlayer currentTurn { get; private set; }
    public GameStatus status { get; private set; }
    private List<Move> _movesPlayed;

    public Game(GenericPlayer p1, GenericPlayer p2, Board board)
    {
        _players = new GenericPlayer[2];
        _players[0] = p1;
        _players[1] = p2;

        this.board = board;

        currentTurn = p1;

        _movesPlayed = new List<Move>();
    }

    public void SubmitMove(Vector2 start, Vector2 end)
    {
        Tile startTile = board.GetTile(start);
        Tile endTile = board.GetTile(end);
    }

    public bool IsEnd() => status != GameStatus.ACTIVE;

    public void SetStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public void ShowMoves(Vector2 selectedPosition)
    {
        List<Vector2> possibleMoves = board
            .GetTile(selectedPosition)
            .GetPiece()
            .PossibleMoves(board, selectedPosition);

        foreach (Vector2 move in possibleMoves)
            board.GetTile(move).Highlight();
    }

    public void ResetHighlights(Vector2 previousPosition)
    {
        List<Vector2> existingMoves = board
            .GetTile(previousPosition)
            .GetPiece()
            .PossibleMoves(board, previousPosition);

        foreach (Vector2 move in existingMoves)
            board.GetTile(move).ResetEffects();
    }
}
