using System.Collections.Generic;

namespace Data
{
    public static class PieceBaseStats
    {
        public static Dictionary<PieceLabel, Piece> data = new Dictionary<PieceLabel, Piece>()
        {
            [PieceLabel.Esa] = new Piece(PieceLabel.Esa, PieceColor.None, 'G', 1124, 1, 1, 3),
            [PieceLabel.PhoenixKnight] = new Piece(PieceLabel.PhoenixKnight, PieceColor.None, 'R', 594, 1, 1.1f, 4),
            [PieceLabel.DarkOne] = new Piece(PieceLabel.DarkOne, PieceColor.None, 'D', 777, 1, 1.15f, 5),
            [PieceLabel.Elder] = new Piece(PieceLabel.Elder, PieceColor.None, 'N', 666, 1, 1.2f, 5),
            [PieceLabel.AngelOfEden] = new Piece(PieceLabel.AngelOfEden, PieceColor.None, 'W', 777, 1, 1.15f, 5),
            [PieceLabel.AbyssLord] = new Piece(PieceLabel.AbyssLord, PieceColor.None, 'B', 594, 1, 1.1f, 4),
            [PieceLabel.Iron] = new Piece(PieceLabel.Iron, PieceColor.None, 'Y', 1124, 1, 1, 3)
        };
    }

    public class Piece
    {
        public PieceLabel label;
        public PieceColor color;
        public char element;
        public float health;
        public float maxHealth;
        public int level;
        public float attack;
        public int moveDistance;
        public string currentSpellEffect = "";

        public PlayerToken player;

        public Piece(PieceLabel _label, PieceColor _color, char _element, float _health, int _level, float _attack, int _moveDistance)
        {
            label = _label;
            color = _color;
            maxHealth = _health;
            health = _health;
            level = _level;
            attack = _attack;
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
            level = PieceBaseStats.data[_label].level;
            attack = PieceBaseStats.data[_label].attack;
            moveDistance = PieceBaseStats.data[_label].moveDistance;
            player = _player;
        }

        public Piece(PieceLabel _label, PieceColor _color, char _element, float _health, float _maxHealth, int _level, float _attack, int _moveDistance, PlayerToken _player, string _currentSpellEffect)
        {
            label = _label;
            color = _color;
            maxHealth = _maxHealth;
            health = _health;
            level = _level;
            attack = _attack;
            moveDistance = _moveDistance;
            player = _player;
            currentSpellEffect = _currentSpellEffect;
        }

        public Piece Clone()
        {
            return new Piece(this.label, this.color, this.element, this.health, this.maxHealth, this.level, this.attack, this.moveDistance, this.player, this.currentSpellEffect);
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