using System.Collections.Generic;

namespace Data
{
    public static class PieceBaseStats
    {
        public static Dictionary<PieceLabel, Piece> data = new Dictionary<PieceLabel, Piece>()
        {
            [PieceLabel.Esa] = new Piece(PieceLabel.Esa, PieceColor.None, 'G', 1124, 1, 112, 400, 3),
            [PieceLabel.PhoenixKnight] = new Piece(PieceLabel.PhoenixKnight, PieceColor.None, 'R', 594, 1, 212, 86, 4),
            [PieceLabel.DarkOne] = new Piece(PieceLabel.DarkOne, PieceColor.None, 'D', 777, 1, 155, 186, 5),
            [PieceLabel.Elder] = new Piece(PieceLabel.Elder, PieceColor.None, 'N', 666, 1, 242, 150, 5),
            [PieceLabel.AngelOfEden] = new Piece(PieceLabel.AngelOfEden, PieceColor.None, 'W', 777, 1, 155, 186, 5),
            [PieceLabel.AbyssLord] = new Piece(PieceLabel.AbyssLord, PieceColor.None, 'B', 594, 1, 212, 86, 4),
            [PieceLabel.Iron] = new Piece(PieceLabel.Iron, PieceColor.None, 'Y', 1124, 1, 112, 400, 3)
        };
    }

    public class Piece
    {
        public PieceLabel label;
        public PieceColor color;
        public char element;
        public int health;
        public int level;
        public int attack;
        public int defense;
        public int moveDistance;

        public PlayerToken player;

        public Piece(PieceLabel _label, PieceColor _color, char _element, int _health, int _level, int _attack, int _defense, int _moveDistance)
        {
            label = _label;
            color = _color;
            health = _health;
            level = _level;
            attack = _attack;
            defense = _defense;
            moveDistance = _moveDistance;
            player = PlayerToken.NA;
        }

        public Piece(PieceLabel _label, PieceColor _color, PlayerToken _player)
        {
            label = _label;
            color = _color;
            health = PieceBaseStats.data[_label].health;
            element = PieceBaseStats.data[_label].element;
            level = PieceBaseStats.data[_label].level;
            attack = PieceBaseStats.data[_label].attack;
            defense = PieceBaseStats.data[_label].defense;
            moveDistance = PieceBaseStats.data[_label].moveDistance;
            player = _player;
        }
    }

    public enum PieceLabel
    {
        AbyssLord,
        AngelOfEden,
        DarkOne,
        Elder,
        Esa,
        Iron,
        PhoenixKnight,
        None
    }

    public enum PieceColor
    {
        Black,
        White,
        None
    }
}