namespace Objects
{
    public class WaterEnv : Environment
    {
        public WaterEnv() : base(remainingLife: 2)
        {
            isTraversable = true;
            damagesOccupant = false;
            healsOccupant = true;
            damageAmount = 0;
            healAmount = 10;
        }
    }
}
