using System.Collections.Generic;

namespace Objects
{
    public class Move
    {
        private readonly GenericPlayer _player;
        private readonly Tile _start;
        private readonly Tile _end;
        private readonly Piece _pieceMoved;
        private readonly Element _elementUsed;
        private readonly List<Piece> _piecesKilled;

        public Move(GenericPlayer player, Tile start, Tile end, Piece pieceMoved, Element elementUsed, List<Piece> piecesKilled)
        {
            _player = player;
            _start = start;
            _end = end;
            _pieceMoved = pieceMoved;
            _elementUsed = elementUsed;
            _piecesKilled = piecesKilled;
        }

        public GenericPlayer player { get { return _player; } }
        public Tile start { get { return _start; } }
        public Tile end { get { return _end; } }
        public Piece pieceMoved { get { return _pieceMoved; } }
        public Element spellCast { get { return _elementUsed; } }
        public List<Piece> piecesKilled { get { return _piecesKilled; } }
    }
}