namespace Objects
{
    public class RockENv : Environment
    {
        public RockENv() : base(remainingLife: 2)
        {
            isTraversable = false;
            damagesOccupant = false;
            healsOccupant = false;
            damageAmount = 0;
            healAmount = 0;
        }
    }
}
