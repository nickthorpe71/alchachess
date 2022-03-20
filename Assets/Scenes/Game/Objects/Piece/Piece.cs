using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public abstract class Piece
    {
        private bool _isDead;
        private readonly bool _isGold;
        private readonly Guid _guid;
        private float _health;
        private float _maxHealth;
        private float _power;
        private int _moveDistance;
        private List<Vector2> _movePattern;


        public Piece(bool isGold)
        {
            _isDead = false;
            _isGold = isGold;
            _guid = Guid.NewGuid();
        }

        public void Damage(int amount)
        {
            _health = Mathf.Max(_health - amount, 0);
        }

        public void Heal(int amount)
        {
            _health = Mathf.Min(_health + amount, _maxHealth);
        }

        public bool isGold { get { return _isGold; } }
        public Guid guid { get { return _guid; } }
        public bool isDead { get { return _isDead; } set { _isDead = value; } }
        public float health { get { return _health; } protected set { _health = value; } }
        public float maxHealth { get { return _maxHealth; } protected set { _maxHealth = value; } }
        public float power { get { return _power; } protected set { _power = value; } }
        public int moveDistance { get { return _moveDistance; } protected set { _moveDistance = value; } }
        public List<Vector2> movePattern { get { return _movePattern; } protected set { _movePattern = value; } }

    }
}
