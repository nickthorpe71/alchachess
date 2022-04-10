using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private GenericPlayer[] _players;
    public Board board { get; private set; }
    public GenericPlayer currentTurn { get; private set; }
    public GameStatus status { get; private set; }
    private List<Move> _movesPlayed;
    public bool localPlayerCanInput { get; private set; }

    public Game(GenericPlayer p1, GenericPlayer p2, Board board)
    {
        _players = new GenericPlayer[2];
        _players[0] = p1;
        _players[1] = p2;

        this.board = board;

        currentTurn = p1;

        _movesPlayed = new List<Move>();
        SetCanInput();
    }

    public List<Tile> GetTilePiecesForPlayer(GenericPlayer player)
    {
        List<Tile> tilesWithPieces = new List<Tile>();
        board.LoopBoard(tile =>
        {
            if (tile.HasPiece() && (tile.GetPiece().isGold == player.isGoldSide))
            {
                tilesWithPieces.Add(tile);
            }
        });

        return tilesWithPieces;
    }

    public void SubmitMove(Vector2 start, Vector2 end)
    {
        Tile startTile = board.GetTile(start);
        Tile endTile = board.GetTile(end);

        bool validMove = startTile.GetPiece().PossibleMoves(board, start).Contains(end);
        if (!validMove) return;

        localPlayerCanInput = false;
        Move move = new Move(currentTurn, startTile, endTile);
        _movesPlayed.Add(move);
        startTile.TransferPiece(to: endTile);
        SetAOEMarkers(endTile.pos, deactivate: true);
    }

    public bool IsEnd() => status != GameStatus.ACTIVE;

    public void NextTurn()
    {
        currentTurn = (currentTurn == _players[0]) ? _players[1] : _players[0];
        SetCanInput();

        if (!currentTurn.isHumanPlayer)
        {
            currentTurn.TakeTurn(this);
        }

        // TODO: FOR TESTING ONLY
        localPlayerCanInput = true;
    }

    private void SetCanInput()
    {
        localPlayerCanInput = currentTurn.isLocalPlayer;
    }

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

        if (hoveredTile.HasActiveElement())
        {
            Element element = hoveredTile.element.GetComponent<Element>();

            foreach (Vector2 aoe in board.ValidateSpellPattern(element.spellPattern, pos))
                board.GetTile(aoe).AOE(deactivate);
        }
    }
}
