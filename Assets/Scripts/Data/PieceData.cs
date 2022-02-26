using UnityEngine;
using System.Collections.Generic;
using System;

namespace Data
{
    public class Piece
    {
        private readonly Guid _guid;
        private readonly PieceLabel _label;
        private readonly PieceColor _color;
        private readonly string _currentRecipe;
        private readonly PlayerToken _player;
        private readonly float _health;
        private readonly float _maxHealth;
        private readonly float _power;
        private readonly int _moveDistance;
        private readonly Vector2[] _movePattern;
        private readonly int _attackDistance;
        private readonly Vector2[] _attackPattern;


        public Piece(Guid guid, PieceLabel label, PieceColor color, PlayerToken player, string currentRecipe, float health, float maxHealth, float power, int moveDistance, Vector2[] movePattern, int attackDistance, Vector2[] attackPattern)
        {
            _guid = guid;
            _label = label;
            _color = color;
            _player = player;
            _currentRecipe = currentRecipe;
            _health = health;
            _maxHealth = maxHealth;
            _power = power;
            _moveDistance = moveDistance;
            _movePattern = movePattern;
            _attackDistance = attackDistance;
            _attackPattern = attackPattern;

        }

        public Guid Guid { get { return _guid; } }
        public PieceLabel Label { get { return _label; } }
        public PieceColor Color { get { return _color; } }
        public PlayerToken Player { get { return _player; } }
        public string CurrentRecipe { get { return _currentRecipe; } }
        public float Health { get { return _health; } }
        public float MaxHealth { get { return _maxHealth; } }
        public float Power { get { return _power; } }
        public int MoveDistance { get { return _moveDistance; } }
        public Vector2[] MovePattern { get { return _movePattern; } }
        public int AttackDistance { get { return _attackDistance; } }
        public Vector2[] AttackPattern { get { return _attackPattern; } }
    }

    public static class PieceTemplates
    {
        public static Dictionary<PieceLabel, Piece> list = new Dictionary<PieceLabel, Piece>()
        {
            {PieceLabel.Knight, new Piece(Guid.NewGuid(), PieceLabel.Knight, PieceColor.None, PlayerToken.NA, "", 3000, 3000, 1.175f, 3, new Vector2[4]{new Vector2(0,1), new Vector2(1,0), new Vector2(0, -1), new Vector2(-1, 0)}, 2, new Vector2[2]{new Vector2(1,0), new Vector2(-1,0)})},
            {PieceLabel.Witch, new Piece(Guid.NewGuid(), PieceLabel.Witch, PieceColor.None, PlayerToken.NA, "", 1450, 1450, 2.5f, 3, new Vector2[4]{new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1)}, 3, new Vector2[4]{new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1)})},
            {PieceLabel.Gargoyle, new Piece(Guid.NewGuid(), PieceLabel.Gargoyle, PieceColor.None, PlayerToken.NA, "", 1900, 1900, 1.5f, 5, new Vector2[8]{new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1)}, 3, new Vector2[8]{new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1,1), new Vector2(1,-1), new Vector2(-1, -1), new Vector2(-1, 1)})},
            {PieceLabel.Demon, new Piece(Guid.NewGuid(), PieceLabel.Demon, PieceColor.None, PlayerToken.NA, "", 2000, 2000, 2f, 4, new Vector2[4]{new Vector2(0,1), new Vector2(1,0), new Vector2(0, -1), new Vector2(-1, 0)}, 2, new Vector2[4]{new Vector2(0,1), new Vector2(1,0), new Vector2(0, -1), new Vector2(-1, 0)})},
        };
    }

    public enum PieceLabel
    {
        Demon,
        Witch,
        Gargoyle,
        Knight,
        None
    }

    public enum PieceColor
    {
        Black,
        White,
        None
    }
}