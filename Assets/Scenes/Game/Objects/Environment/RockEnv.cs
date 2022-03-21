namespace Objects
{
    public class RockEnv : Environment
    {
        public RockEnv() : base(remainingLife: 2)
        {
            isTraversable = false;
            damagesOccupant = false;
            healsOccupant = false;
            damageAmount = 0;
            healAmount = 0;
        }
    }
}
