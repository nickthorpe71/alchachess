namespace Objects
{
    public abstract class GenericPlayer
    {
        private bool _isGoldSide;
        private bool _isHumanPlayer;

        public bool isGoldSide { get { return _isGoldSide; } set { _isGoldSide = value; } }
        public bool isHumanPlayer { get { return _isHumanPlayer; } set { _isHumanPlayer = value; } }
    }
}
