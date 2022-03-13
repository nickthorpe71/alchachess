using UnityEngine;

namespace Data
{
    public class Board
    {
        public Tile[][] tiles;

        public Board()
        {
            tiles = new Tile[Const.BOARD_HEIGHT][];

            for (int y = 0; y < Const.BOARD_HEIGHT; y++)
            {
                tiles[y] = new Tile[Const.BOARD_WIDTH];
                for (int x = 0; x < Const.BOARD_WIDTH; x++)
                    tiles[y][x] = new Tile(x, y);
            }
        }
    }

    public class Tile
    {
        private readonly TileContents _contents;
        private readonly Piece _piece;
        private readonly string _element;
        private readonly int _x;
        private readonly int _y;
        private readonly int _remainingTimeOnEnvironment = 0;

        // States
        private readonly bool _isClicked = false;
        private readonly bool _isHovered = false;
        private readonly bool _isHighlighted = false;
        private readonly bool _isAOE = false;

        public Tile(int x, int y)
        {
            _contents = TileContents.Empty;
            _piece = null;
            _element = "N";
            _x = x;
            _y = y;
        }

        public Tile(int x, int y,
            Piece piece,
            string element,
            TileContents contents,
            bool isClicked,
            bool isHovered,
            bool isHighlighted,
            bool isAOE,
            int remainingTimeOnEnvironment
        )
        {
            _x = x;
            _y = y;
            _piece = piece;
            _element = element;
            _contents = contents;
            _isClicked = isClicked;
            _isHovered = isHovered;
            _isHighlighted = isHighlighted;
            _isAOE = isAOE;
            _remainingTimeOnEnvironment = remainingTimeOnEnvironment;
        }

        public TileContents Contents { get { return _contents; } }
        public Piece Piece { get { return _piece; } }
        public string Element { get { return _element; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int RemainingTimeOnEnvironment { get { return _remainingTimeOnEnvironment; } }

        // States
        public bool IsClicked { get { return _isClicked; } }
        public bool IsHovered { get { return _isHovered; } }
        public bool IsHighlighted { get { return _isHighlighted; } }
        public bool IsAOE { get { return _isAOE; } }

    }

    public class MoveData
    {
        private readonly PlayerToken _actingPlayer;
        private readonly Piece _pieceMoved;
        private readonly Vector2 _pieceStart;
        private readonly Vector2 _pieceEnd;
        private readonly Spell _spellCast;
        private readonly Board _boardPreMove;
        private readonly Board _boardPostMove;

        public MoveData(
            PlayerToken actingPlayer,
            Piece pieceMoved,
            Vector2 pieceStart,
            Vector2 pieceEnd,
            Board boardPreMove,
            Board boardPostMove,
            Spell spellCast
        )
        {
            _actingPlayer = actingPlayer;
            _pieceMoved = pieceMoved;
            _pieceStart = pieceStart;
            _pieceEnd = pieceEnd;
            _boardPreMove = boardPreMove;
            _boardPostMove = boardPostMove;
            _spellCast = spellCast;
        }

        public PlayerToken ActingPlayer { get { return _actingPlayer; } }
        public Piece PieceMoved { get { return _pieceMoved; } }
        public Vector2 PieceStart { get { return _pieceStart; } }
        public Vector2 PieceEnd { get { return _pieceEnd; } }
        public Board BoardPreMove { get { return _boardPreMove; } }
        public Board BoardPostMove { get { return _boardPostMove; } }
        public Spell SpellCast { get { return _spellCast; } }
    }

    public enum TileContents
    {
        Piece,
        Element,
        Environment,
        Empty
    }

    public enum TileState
    {
        isClicked,
        isHovered,
        isHighlighted,
        isAOE
    }

    public class BalancedElementLayout
    {
        public string[][] pattern = new string[][] {
            new string[] {"R","B","W","W","B","R"},
            new string[] {"G","Y","W","G","Y","D"},
            new string[] {"D","B","Y","D","G","R"},
            new string[] {"R","G","D","Y","B","D"},
            new string[] {"D","Y","G","W","Y","G"},
            new string[] {"R","B","W","W","B","R"},
        };
    }
}
