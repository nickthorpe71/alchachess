using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class Game
    {
        private GenericPlayer[] _players;
        public Board board { get; private set; }
        public GenericPlayer currentTurn { get; private set; }
        public GameStatus status { get; private set; }
        private List<Move> _movesPlayed;

        public Game(GenericPlayer p1, GenericPlayer p2)
        {
            _players = new GenericPlayer[2];
            _players[0] = p1;
            _players[1] = p2;

            board = new Board(6, 6);

            currentTurn = (p1.isGoldSide) ? p1 : p2;

            _movesPlayed = new List<Move>();
        }

        public void SubmitMove(Vector2 start, Vector2 end)
        {
            Tile startTile = board.GetTile(start);
            Tile endTile = board.GetTile(end);
            UnityEngine.Debug.Log(startTile.AsString());
            UnityEngine.Debug.Log(endTile.AsString());
            UnityEngine.Debug.Log($"move {startTile.piece.GetType().Name} to {endTile.x},{endTile.y}");
        }

        public bool IsEnd() => status != GameStatus.ACTIVE;

        public void SetStatus(GameStatus newStatus)
        {
            status = newStatus;
        }
    }
}
