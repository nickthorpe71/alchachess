using UnityEngine;

namespace Data
{
    public class Board
    {
        public Tile[][] tiles;

        public Board()
        {
            tiles = new Tile[Const.BOARD_HEIGHT][];

            tiles[0] = new Tile[] {
                new Tile(0, 0),
                new Tile(1, 0, PieceLabel.Esa, PieceColor.Black, PlayerToken.P1),
                new Tile(2, 0, PieceLabel.PhoenixKnight, PieceColor.Black, PlayerToken.P1),
                new Tile(3, 0, PieceLabel.AngelOfEden, PieceColor.Black, PlayerToken.P1),
                new Tile(4, 0, PieceLabel.DarkOne, PieceColor.Black, PlayerToken.P1),
                new Tile(5, 0, PieceLabel.AbyssLord, PieceColor.Black, PlayerToken.P1),
                new Tile(6, 0, PieceLabel.Iron, PieceColor.Black, PlayerToken.P1),
                new Tile(7, 0)
                };
            tiles[1] = new Tile[] {
                new Tile(0, 1 ),
                new Tile(1, 1,"G"),
                new Tile(2, 1,"R"),
                new Tile(3, 1,"W"),
                new Tile(4, 1,"D"),
                new Tile(5, 1,"B"),
                new Tile(6, 1,"Y"),
                new Tile(7, 1 )
                };
            tiles[2] = new Tile[] {
                new Tile(0, 2 ),
                new Tile(1, 2,"D"),
                new Tile(2, 2,"Y"),
                new Tile(3, 2,"B"),
                new Tile(4, 2,"R"),
                new Tile(5, 2,"G"),
                new Tile(6, 2,"W"),
                new Tile(7, 2 )
                };
            tiles[3] = new Tile[] {
                new Tile(0, 3 ),
                new Tile(1, 3,"R"),
                new Tile(2, 3,"W"),
                new Tile(3, 3,"G"),
                new Tile(4, 3,"Y"),
                new Tile(5, 3,"D"),
                new Tile(6, 3,"B"),
                new Tile(7, 3 )
                };
            tiles[4] = new Tile[] {
                new Tile(0, 4 ),
                new Tile(1, 4,"B"),
                new Tile(2, 4,"D"),
                new Tile(3, 4,"Y"),
                new Tile(4, 4,"G"),
                new Tile(5, 4,"W"),
                new Tile(6, 4,"R"),
                new Tile(7, 4 )
                };
            tiles[5] = new Tile[] {
                new Tile(0, 5 ),
                new Tile(1, 5,"W"),
                new Tile(2, 5,"G"),
                new Tile(3, 5,"R"),
                new Tile(4, 5,"B"),
                new Tile(5, 5,"Y"),
                new Tile(6, 5,"D"),
                new Tile(7, 5 )
                };
            tiles[6] = new Tile[] {
                new Tile(0, 6 ),
                new Tile(1, 6,"Y"),
                new Tile(2, 6,"B"),
                new Tile(3, 6,"D"),
                new Tile(4, 6,"W"),
                new Tile(5, 6,"R"),
                new Tile(6, 6,"G"),
                new Tile(7, 6 )
                };
            tiles[7] = new Tile[] {
                new Tile(0, 7 ),
                new Tile(1, 7,PieceLabel.Iron, PieceColor.White, PlayerToken.P2),
                new Tile(2, 7,PieceLabel.AbyssLord, PieceColor.White, PlayerToken.P2),
                new Tile(3, 7,PieceLabel.DarkOne, PieceColor.White, PlayerToken.P2),
                new Tile(4, 7,PieceLabel.AngelOfEden, PieceColor.White, PlayerToken.P2),
                new Tile(5, 7,PieceLabel.PhoenixKnight, PieceColor.White, PlayerToken.P2),
                new Tile(6, 7,PieceLabel.Esa, PieceColor.White, PlayerToken.P2),
                new Tile(7, 7 )
                };
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

        public Tile(int x, int y, PieceLabel pieceLabel, PieceColor pieceColor, PlayerToken player)
        {
            _contents = TileContents.Piece;
            _piece = new Piece(pieceLabel, pieceColor, player);
            _element = "N";
            _x = x;
            _y = y;
        }

        public Tile(int x, int y, string element)
        {
            _contents = TileContents.Element;
            _piece = null;
            _element = element;
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
}
