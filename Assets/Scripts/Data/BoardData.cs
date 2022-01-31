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
        public TileContents contents;
        public Piece piece;
        public string element;
        public int x;
        public int y;
        public int remainingTimeOnEnvironment = 0;

        // States
        public bool isClicked = false;
        public bool isHovered = false;
        public bool isHighlighted = false;
        public bool isAOE = false;

        public Tile(int _x, int _y)
        {
            contents = TileContents.Empty;
            piece = null;
            element = "N";
            x = _x;
            y = _y;
        }

        public Tile(int _x, int _y, PieceLabel peiceLabel, PieceColor pieceColor, PlayerToken player)
        {
            contents = TileContents.Piece;
            piece = new Piece(peiceLabel, pieceColor, player);
            element = "N";
            x = _x;
            y = _y;
        }

        public Tile(int _x, int _y, string _element)
        {
            contents = TileContents.Element;
            piece = null;
            element = _element;
            x = _x;
            y = _y;
        }

        public Tile(int _x, int _y,
            Piece _piece,
            string _element,
            TileContents _contents,
            bool _isClicked,
            bool _isHovered,
            bool _isHighlighted,
            bool _isAOE,
            int _remainingTimeOnEnvironment
        )
        {
            x = _x;
            y = _y;
            piece = _piece;
            element = _element;
            contents = _contents;
            isClicked = _isClicked;
            isHovered = _isHovered;
            isHighlighted = _isHighlighted;
            isAOE = _isAOE;
            remainingTimeOnEnvironment = _remainingTimeOnEnvironment;
        }

        public Tile Clone()
        {
            return new Tile(x, y)
            {
                x = this.x,
                y = this.y,
                piece = this.piece == null ? null : this.piece.Clone(),
                element = this.element,
                contents = this.contents,
                isClicked = this.isClicked,
                isHovered = this.isHovered,
                isHighlighted = this.isHighlighted,
                isAOE = this.isAOE,
                remainingTimeOnEnvironment = this.remainingTimeOnEnvironment
            };
        }
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
