namespace Objects
{
    public class PlantEnv : Environment
    {
        public PlantEnv() : base(remainingLife: 2)
        {
            isTraversable = false;
            damagesOccupant = false;
            healsOccupant = false;
            damageAmount = 0;
            healAmount = 0;
        }
    }
}
