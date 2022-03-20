using Logic;

namespace Objects
{
    public class Tile
    {
        private readonly int _x;
        private readonly int _y;
        private Piece _piece;
        private Environment _environment;
        private readonly Element _element;

        public Tile(int x, int y, Piece piece, Element element)
        {
            _x = x;
            _y = y;
            _piece = piece;
            _element = element;
            _environment = null;
        }

        public string AsString() => $"x:{_x} y:{_y}\nP:{Helpers.GetClassAsStr(_piece)}\nEnv:{Helpers.GetClassAsStr(_environment)}\nEl:{Helpers.GetClassAsStr(_element)}";

        public bool CanTraverse() => _environment == null && _piece == null;

        public int x { get { return _x; } }
        public int y { get { return _y; } }
        public Piece piece { get { return _piece; } set { _piece = value; } }
        public Environment environment { get { return _environment; } set { _environment = value; } }
        public Element element { get { return _element; } }
    }
}

