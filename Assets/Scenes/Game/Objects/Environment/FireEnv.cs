namespace Objects
{
    public class FireEnv : Environment
    {
        public FireEnv() : base(remainingLife: 2)
        {
            isTraversable = true;
            damagesOccupant = true;
            healsOccupant = false;
            damageAmount = 10;
            healAmount = 0;
        }
    }
}
