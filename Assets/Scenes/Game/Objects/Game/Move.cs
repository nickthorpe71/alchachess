namespace Objects
{
    public class Move
    {
        private readonly GenericPlayer _player;
        public GenericPlayer player { get { return _player; } }
        private readonly Tile _start;
        public Tile start { get { return _start; } }
        private readonly Tile _end;
        public Tile end { get { return _end; } }

        public Move(GenericPlayer player, Tile start, Tile end)
        {
            _player = player;
            _start = start;
            _end = end;
        }
    }
}