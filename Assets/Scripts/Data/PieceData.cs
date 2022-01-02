using System;
using System.Reflection;
using System.Collections.Generic;

namespace Data
{
    public static class PieceBaseStats
    {
        public static Dictionary<PieceLabel, Piece> data = new Dictionary<PieceLabel, Piece>()
        {
            [PieceLabel.Esa] = new Piece(PieceLabel.Esa, PieceColor.None, 'G', 1124, 1, 3),
            [PieceLabel.PhoenixKnight] = new Piece(PieceLabel.PhoenixKnight, PieceColor.None, 'R', 594, 1.1f, 4),
            [PieceLabel.DarkOne] = new Piece(PieceLabel.DarkOne, PieceColor.None, 'D', 777, 1.15f, 5),
            [PieceLabel.Elder] = new Piece(PieceLabel.Elder, PieceColor.None, 'N', 666, 1.2f, 5),
            [PieceLabel.AngelOfEden] = new Piece(PieceLabel.AngelOfEden, PieceColor.None, 'W', 777, 1.15f, 5),
            [PieceLabel.AbyssLord] = new Piece(PieceLabel.AbyssLord, PieceColor.None, 'B', 594, 1.1f, 4),
            [PieceLabel.Iron] = new Piece(PieceLabel.Iron, PieceColor.None, 'Y', 1124, 1, 3)
        };
    }

    public class Piece
    {
        public PieceLabel label;
        public PieceColor color;
        public Char element;
        public float health;
        public float maxHealth;
        public float level;
        public float experience;
        public float attack;
        public int moveDistance;

        public string currentSpellEffect = "";

        public PlayerToken player;

        // this is an un-owned piece
        public Piece(PieceLabel _label, PieceColor _color, char _element, float _health, float _attack, int _moveDistance)
        {
            label = _label;
            color = _color;
            maxHealth = _health;
            health = _health;
            level = 1;
            experience = 0;
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
            experience = 0;
            attack = PieceBaseStats.data[_label].attack;
            moveDistance = PieceBaseStats.data[_label].moveDistance;
            player = _player;
        }

        public Piece(PieceLabel _label, PieceColor _color, char _element, float _health, float _maxHealth, float _level, float _experience, float _attack, int _moveDistance, PlayerToken _player, string _currentSpellEffect)
        {
            label = _label;
            color = _color;
            maxHealth = _maxHealth;
            health = _health;
            level = _level;
            experience = _experience;
            attack = _attack;
            moveDistance = _moveDistance;
            player = _player;
            currentSpellEffect = _currentSpellEffect;
        }

        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(Piece);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(Piece);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }

        public Piece Clone()
        {
            return new Piece(this.label, this.color, this.element, this.health, this.maxHealth, this.level, this.experience, this.attack, this.moveDistance, this.player, this.currentSpellEffect);
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