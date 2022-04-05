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

        // track move
        Move move = new Move(currentTurn, startTile, endTile);
        _movesPlayed.Add(move);

        startTile.TransferPiece(to: endTile);

        // remove tile effects from board
        endTile.Hover(deactivate: true);
        endTile.Click(deactivate: true);
        SetAOEMarkers(endTile.pos, deactivate: true);
    }

    public bool IsEnd() => status != GameStatus.ACTIVE;

    public void SetStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public void SetHighlightedMoves(Vector2 pos, bool deactivate = false)
    {
        Tile tile = board.GetTile(pos);
        if (!tile.HasPiece()) return;

        List<Vector2> possibleMoves = tile
            .GetPiece()
            .PossibleMoves(board, pos);

        foreach (Vector2 move in possibleMoves)
            board.GetTile(move).Highlight(deactivate);
    }

    public void SetAOEMarkers(Vector2 pos, bool deactivate = false)
    {
        Tile hoveredTile = board.GetTile(pos);
        Element element = hoveredTile.element.GetComponent<Element>();

        foreach (Vector2 aoe in element.GetSpellPattern(board, pos))
            board.GetTile(aoe).AOE(deactivate);
    }
}
