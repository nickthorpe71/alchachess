namespace Data
{
    public class ScoredMove
    {
        private readonly Tile _start;
        private readonly Tile _end;
        private readonly Spell _spell;
        private readonly float _score;

        public ScoredMove(Tile start, Tile end, Spell spell, float score)
        {
            _start = start;
            _end = end;
            _spell = spell;
            _score = score;
        }

        public Tile Start { get { return _start; } }

        public Tile End { get { return _end; } }

        public Spell Spell { get { return _spell; } }

        public float Score { get { return _score; } }
    }
}