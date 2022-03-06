using UnityEngine;
using System.Collections.Generic;
using System;

namespace Data
{
    public class Piece
    {
        private readonly Guid _guid;
        private readonly PieceLabel _label;
        private readonly GodType _godType;
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


        public Piece(Guid guid, PieceLabel label, GodType godType, PieceColor color, PlayerToken player, string currentRecipe, float health, float maxHealth, float power, int moveDistance, Vector2[] movePattern, int attackDistance, Vector2[] attackPattern)
        {
            _guid = guid;
            _label = label;
            _godType = godType;
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
        public GodType GodType { get { return _godType; } }
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
            [PieceLabel.Knight] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Knight,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 30,
                maxHealth: 30,
                power: 1,
                moveDistance: 3,
                movePattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) },
                attackDistance: 4,
                attackPattern: new Vector2[2] { new Vector2(1, 0), new Vector2(-1, 0) }
            ),
            [PieceLabel.Witch] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Witch,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 14,
                maxHealth: 14,
                power: 3,
                moveDistance: 4,
                movePattern: new Vector2[4] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) },
                attackDistance: 5,
                attackPattern: new Vector2[4] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) }
            ),
            [PieceLabel.Gargoyle] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Gargoyle,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 19,
                maxHealth: 19,
                power: 1,
                moveDistance: 5,
                movePattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) },
                attackDistance: 1,
                attackPattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) }
            ),
            [PieceLabel.Demon] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Demon,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 22,
                maxHealth: 22,
                power: 2,
                moveDistance: 4,
                movePattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) },
                attackDistance: 2,
                attackPattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) }
            ),
            [PieceLabel.Jester] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Jester,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 35,
                maxHealth: 35,
                power: 2,
                moveDistance: 5,
                movePattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) },
                attackDistance: 1,
                attackPattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) }
            ),
            [PieceLabel.AncientArcher] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.AncientArcher,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 32,
                maxHealth: 32,
                power: 2,
                moveDistance: 3,
                movePattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) },
                attackDistance: 5,
                attackPattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) }
            ),
            [PieceLabel.Wraith] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Wraith,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 30,
                maxHealth: 30,
                power: 3,
                moveDistance: 3,
                movePattern: new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) },
                attackDistance: 3,
                attackPattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) }
            ),
            [PieceLabel.GodOfLife] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.GodOfLife,
                godType: GodType.Life,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 50,
                maxHealth: 50,
                power: 1,
                moveDistance: 2,
                movePattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) },
                attackDistance: 1,
                attackPattern: new Vector2[8] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) }
            ),
        };
    }

    public enum PieceLabel
    {
        Demon,
        Witch,
        Gargoyle,
        Knight,
        Jester,
        AncientArcher,
        Wraith,
        GodOfLife,
        None
    }

    public enum GodType
    {
        Demi,
        Death,
        Life
    }

    public enum PieceColor
    {
        Black,
        White,
        None
    }
}