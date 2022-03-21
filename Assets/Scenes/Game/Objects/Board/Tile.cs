using Logic;

namespace Objects
{
    public class Tile
    {
        private readonly int _x;
        public int x { get { return _x; } }
        private readonly int _y;
        public int y { get { return _y; } }
        public Piece piece { get; private set; }
        public Environment environment { get; private set; }
        public Element element { get; private set; }

        public Tile(int x, int y, Piece piece, Element element)
        {
            _x = x;
            _y = y;
            this.piece = piece;
            this.element = element;
            this.environment = null;
        }

        public string AsString() => $"x:{_x} y:{_y}\nP:{Helpers.GetClassAsStr(piece)}\nEnv:{Helpers.GetClassAsStr(environment)}\nEl:{Helpers.GetClassAsStr(element)}";

        public bool CanTraverse() => (environment == null || !environment.isTraversable) && piece == null;
    }
}

