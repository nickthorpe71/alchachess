namespace Objects
{
    public class Environment
    {
        public int remainingLife { get; private set; }
        public bool isTraversable { get; protected set; }
        public bool damagesOccupant { get; protected set; }
        public bool healsOccupant { get; protected set; }
        public int damageAmount { get; protected set; }
        public int healAmount { get; protected set; }

        public Environment(int remainingLife)
        {
            this.remainingLife = remainingLife;
        }

        public void DecrementLife()
        {
            remainingLife -= 1;
        }

    }
}
