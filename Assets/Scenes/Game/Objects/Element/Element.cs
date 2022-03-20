using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public abstract class Element
    {
        private List<Vector2> _spellPattern;
        private int _spellDamage;
        private bool _damagesEnemies;
        private bool _healsEnemies;
        private bool _damagesAllies;
        private bool _healsAllies;
        private bool _altersEnvironment;
        private int _duration;


        public List<Vector2> spellPattern { get { return _spellPattern; } protected set { _spellPattern = value; } }
        public int spellDamage { get { return _spellDamage; } protected set { _spellDamage = value; } }
        public bool damagesEnemies { get { return _damagesEnemies; } protected set { _damagesEnemies = value; } }
        public bool healsEnemies { get { return _healsEnemies; } protected set { _healsEnemies = value; } }
        public bool damagesAllies { get { return _damagesAllies; } protected set { _damagesAllies = value; } }
        public bool healsAllies { get { return _healsAllies; } protected set { _healsAllies = value; } }
        public bool altersEnvironment { get { return _altersEnvironment; } protected set { _altersEnvironment = value; } }
        public int duration { get { return _duration; } protected set { _duration = value; } }
    }
}
