namespace OprBattleSim.Model
{
    internal class HitResult
    {

        public int Damage;
        internal int AttackRollResult;
        internal int DefenseRollResult;

        public  HitResult(int damage)
        {
            Damage = damage;
        }
    }
}