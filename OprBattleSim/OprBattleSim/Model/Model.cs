namespace OprBattleSim.Model
{
    public class Model
    {
        public Unit Unit;
        public List<Weapon> Weapons = new List<Weapon>();
        public int MaxToughness = 1;
        public int CurrentToughness = 1;

        public Random rand = new Random();

        internal void Attack(Unit unit2)
        {
            foreach (Weapon weapon in Weapons)
            {
                for (int i = 0; i < weapon.Attacks; i++)
                {
                    int d = Roll();
                    if (Unit.Quality <= d)
                    {
                        // hit !
                        unit2.Hit();
                    }

                }
            }
        }

        private int Roll()
        {
            return rand.Next(1, 6);
        }

        internal bool IsAlive()
        {
            return CurrentToughness > 0;
        }



    }
}
