using System.Collections.Generic;

namespace Objects
{
    public class Game
    {
        private GenericPlayer[] _players;
        private Board _board;
        private GenericPlayer _currentTurn;
        private GameStatus _status;
        private List<Move> _movesPlayed;

        public Game(GenericPlayer p1, GenericPlayer p2)
        {
            _players = new GenericPlayer[2];
            _players[0] = p1;
            _players[1] = p2;

            _board = new Board(6, 6);

            _currentTurn = (p1.isGoldSide) ? p1 : p2;

            _movesPlayed = new List<Move>();
        }

        public bool IsEnd() => GetStatus() != GameStatus.ACTIVE;
        public GameStatus GetStatus() => _status;
        public void SetStatus(GameStatus status) => _status = status;

        public Board board { get { return _board; } }
    }
}
