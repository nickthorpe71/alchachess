using System.Collections.Generic;

namespace Data
{
    public static class PieceBaseStats
    {
        public static Dictionary<PieceLabel, Piece> data = new Dictionary<PieceLabel, Piece>()
        {
            [PieceLabel.Esa] = new Piece(PieceLabel.Esa, PieceColor.None, "G", 1250, 1.2f, 5),
            [PieceLabel.PhoenixKnight] = new Piece(PieceLabel.PhoenixKnight, PieceColor.None, "R", 850, 1.6f, 5),
            [PieceLabel.DarkOne] = new Piece(PieceLabel.DarkOne, PieceColor.None, "D", 1000, 1.4f, 5),
            [PieceLabel.Elder] = new Piece(PieceLabel.Elder, PieceColor.None, "N", 750, 2f, 6),
            [PieceLabel.AngelOfEden] = new Piece(PieceLabel.AngelOfEden, PieceColor.None, "W", 1000, 1.4f, 5),
            [PieceLabel.AbyssLord] = new Piece(PieceLabel.AbyssLord, PieceColor.None, "B", 850, 1.6f, 5),
            [PieceLabel.Iron] = new Piece(PieceLabel.Iron, PieceColor.None, "Y", 1250, 1.2f, 5)
        };
    }

    public class Piece
    {
        public PieceLabel label;
        public PieceColor color;
        public string element;
        public float health;
        public float maxHealth;
        public float power;
        public int moveDistance;

        public PlayerToken player;

        // this is an un-owned piece
        public Piece(PieceLabel _label, PieceColor _color, string _element, float _health, float _power, int _moveDistance)
        {
            label = _label;
            color = _color;
            element = _element;
            maxHealth = _health;
            health = _health;
            power = _power;
            moveDistance = _moveDistance;
            player = PlayerToken.NA;
        }

        public Piece(PieceLabel _label, PieceColor _color, PlayerToken _player)
        {
            label = _label;
            color = _color;
            health = PieceBaseStats.data[_label].health;
            maxHealth = PieceBaseStats.data[_label].health;
            element = PieceBaseStats.data[_label].element;
            power = PieceBaseStats.data[_label].power;
            moveDistance = PieceBaseStats.data[_label].moveDistance;
            player = _player;
        }

        public Piece(PieceLabel _label, PieceColor _color, string _element, float _health, float _maxHealth, float _power, int _moveDistance, PlayerToken _player)
        {
            label = _label;
            color = _color;
            element = _element;
            maxHealth = _maxHealth;
            health = _health;
            power = _power;
            moveDistance = _moveDistance;
            player = _player;
        }

        public Piece Clone()
        {
            return new Piece(this.label, this.color, this.element, this.health, this.maxHealth, this.power, this.moveDistance, this.player);
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