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
        private readonly List<Direction> _movePattern;


        public Piece(Guid guid, PieceLabel label, GodType godType, PieceColor color, PlayerToken player, string currentRecipe, float health, float maxHealth, float power, int moveDistance, List<Direction> movePattern)
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
        public List<Direction> MovePattern { get { return _movePattern; } }
    }

    public static class PieceTemplates
    {
        public static Dictionary<PieceLabel, Piece> list = new Dictionary<PieceLabel, Piece>()
        {
            [PieceLabel.Witch] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Witch,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 20,
                maxHealth: 20,
                power: 8,
                moveDistance: 3,
                movePattern: new List<Direction> { Direction.NE, Direction.SE, Direction.SW, Direction.NW }
            ),
            [PieceLabel.Knight] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Knight,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 35,
                maxHealth: 35,
                power: 6,
                moveDistance: 3,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W }
            ),
            [PieceLabel.Gargoyle] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Gargoyle,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 15,
                maxHealth: 15,
                power: 1,
                moveDistance: 1,
                movePattern: new List<Direction> { Direction.N, Direction.S }
            ),
            [PieceLabel.Demon] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Demon,
                godType: GodType.Demi,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 35,
                maxHealth: 35,
                power: 8,
                moveDistance: 5,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W }
            ),
            [PieceLabel.Jester] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Jester,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 45,
                maxHealth: 45,
                power: 10,
                moveDistance: 4,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.SE, Direction.SW, Direction.NW }
            ),
            [PieceLabel.AncientArcher] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.AncientArcher,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 30,
                maxHealth: 30,
                power: 20,
                moveDistance: 2,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.SE, Direction.SW, Direction.NW }
            ),
            [PieceLabel.Wraith] = new Piece(
                guid: Guid.NewGuid(),
                label: PieceLabel.Wraith,
                godType: GodType.Death,
                color: PieceColor.None,
                player: PlayerToken.NA,
                currentRecipe: "",
                health: 55,
                maxHealth: 55,
                power: 12,
                moveDistance: 5,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.SE, Direction.SW, Direction.NW }
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
                power: 10,
                moveDistance: 1,
                movePattern: new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.SE, Direction.SW, Direction.NW }
            )
        };
    }

    public enum PieceLabel
    {
        Demon,
        Witch,
        Knight,
        Gargoyle,
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

    public enum Direction
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }
}