using System.Collections.Generic;

namespace Objects
{
    public class Move
    {
        private readonly GenericPlayer _player;
        public GenericPlayer player { get { return _player; } }
        private readonly Tile _start;
        public Tile start { get { return _start; } }
        private readonly Tile _end;
        public Tile end { get { return _end; } }
        private readonly Piece _pieceMoved;
        public Piece pieceMoved { get { return _pieceMoved; } }
        private readonly Element _elementUsed;
        public Element spellCast { get { return _elementUsed; } }
        private readonly List<Piece> _piecesKilled;
        public List<Piece> piecesKilled { get { return _piecesKilled; } }

        public Move(GenericPlayer player, Tile start, Tile end, Piece pieceMoved, Element elementUsed, List<Piece> piecesKilled)
        {
            _player = player;
            _start = start;
            _end = end;
            _pieceMoved = pieceMoved;
            _elementUsed = elementUsed;
            _piecesKilled = piecesKilled;
        }
    }
}