using System.Text.RegularExpressions;

namespace OprBattleSim.Model
{
    public class Model
    {
        public Unit Unit;
        public List<Weapon> Weapons = new List<Weapon>();
        public int MaxToughness = 1;
        public int CurrentToughness = 1;

        static Random Rand = new Random();

        // SkillRegexs
        private readonly Regex ToughRegex = new("Tough\\(([0-9]{1,2})\\)");
        private readonly Regex BadShotRegex = new("Bad Shot");

        // skills
        private bool BadShot = false;

        public Model(Unit unit, string skillStr)
        {
            Unit = unit;
            ParseSkills(skillStr);
        }

        private void ParseSkills(string skillsStr)
        {
            MaxToughness = ApplyIntRegex(ToughRegex, skillsStr, 1);
            CurrentToughness = MaxToughness;
            if (BadShotRegex.IsMatch(skillsStr))
            {
                BadShot = true;
            }
        }

        internal ModelAttackResult Attack(Unit unit2, int distance)
        {
            ModelAttackResult attackResult = new();
            foreach (Weapon weapon in Weapons)
            {
                if (distance == 0 && weapon.Range != 0) continue; // melee! skip ranged weapons
                if (distance > weapon.Range) continue; // ranged! skip out of range weapons

                if (distance > 0 && BadShot)
                {
                    weapon.AttackQuality = 5;
                }
                else
                {
                    weapon.AttackQuality = Unit.Quality;
                }

                for (int i = 0; i < weapon.Attacks; i++)
                {
                    int d = Roll();
                    if (weapon.AttackQuality <= d)
                    {
                        HitResult hitResult = unit2.TakeHit();
                        hitResult.AttackRollResult = d;
                        attackResult.HitResults.Add(hitResult);
                    }
                }
            }
            return attackResult;
        }

        private int Roll()
        {
            return Rand.Next(1, 6);
        }

        internal bool IsAlive()
        {
            return CurrentToughness > 0;
        }

        private int ApplyIntRegex(Regex regex, string str, int noMatchValue = 0)
        {
            var matches = regex.Matches(str);
            if (matches.Count == 0)
            {
                return noMatchValue;
            }
            else
            {
                return Int32.Parse(matches[0].Groups[1].Value);
            }
        }
        private string ApplyStrRegex(Regex regex, string str)
        {
            return regex.Matches(str)[0].Groups[1].Value;
        }

    }
}
