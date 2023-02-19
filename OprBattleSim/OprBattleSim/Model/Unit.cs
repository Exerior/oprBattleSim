using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OprBattleSim.Model
{
    public class Unit
    {
        public string Name = "";
        public int ModelCount = 0;
        public int Quality = 0;
        public int Defense = 0;
        public List<Model> Models = new List<Model>();

        // Unit Regexs
        private Regex nameRegex = new Regex("(.*) \\[");
        private Regex modelCountRegex = new Regex("\\[([0-9]+)\\]");
        private Regex qualityRegex = new Regex(" Q([0-9]{1})\\+ ");
        private Regex defenseRegex = new Regex("D([0-9]{1})\\+");

        // Weapon Regexs
        private Regex weaponCountRegex = new Regex("([0-9]{1,2})x");
        private Regex weaponContentRegex = new Regex("\\((.*)\\)");
        private Regex weaponRangeRegex = new Regex("([0-9]{1,2})\"");
        private Regex weaponAttacksRegex = new Regex("A([0-9]{1,2})");

        Random rand = new Random();

        internal void Attack(Unit unit2)
        {
            foreach (Model model in Models)
            {
                model.Attack(unit2);
            }
        }

        internal void Hit(int ap = 0, int blast = 1, int deadly = 1)
        {
            int d = Roll();
            if (d == 6)
            {
                return;
            }
            if ((d - ap) >= Defense)
            {
                foreach (Model model in Models)
                {
                    if (model.IsAlive())
                    {
                        model.CurrentToughness -= deadly;
                        return;
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
            foreach (var model in Models)
            {
                if (model.IsAlive())
                    return true;
            }
            return false;
        }

        internal void Parse(string unit2String)
        {
            // split the units by doulbe linebreak
            foreach (String strUnit in unit2String.Replace("\n\r", "\n").Split("\n\n"))
            {
                List<String> lines = strUnit.Split("\n").ToList();
                if (lines.Count < 2)
                {
                    continue;
                }
                String line1 = lines[0];
                String line2 = lines[1];
                List<String> line1parts = lines[0].Split(" | ").ToList();
                String baseStatsStr = line1parts[0];
                Name = nameRegex.Matches(baseStatsStr)[0].Groups[1].Value;
                ModelCount = Int32.Parse(modelCountRegex.Matches(baseStatsStr)[0].Groups[1].Value);
                Quality = Int32.Parse(qualityRegex.Matches(baseStatsStr)[0].Groups[1].Value);
                var test = defenseRegex.Matches(baseStatsStr);
                Defense = Int32.Parse(defenseRegex.Matches(baseStatsStr)[0].Groups[1].Value);

                String pointCostsStr = line1parts[1];
                String skillsStr = line1parts[2];

                List<String> weaponsStr = SplitWeapons(lines[1]);

                for (int i = 0; i < ModelCount; i++)
                {
                    Model model = new Model();
                    model.Unit = this;
                    Models.Add(model);
                }

                // wtf is the weapon distribution pain in the ass. For now just give the models thee weapons ... somehow
                int iModel = 0;
                foreach (String strWeapon in weaponsStr)
                {
                    string weaponContent = ApplyStrRegex(weaponContentRegex, strWeapon);
                    int weaponCount = ApplyIntRegex(weaponCountRegex, strWeapon, 1);
                    int weaponRange = ApplyIntRegex(weaponRangeRegex, weaponContent, 0);
                    int weaponAttacks = ApplyIntRegex(weaponAttacksRegex, weaponContent);
                    for (int i = 0; i < weaponCount; i++)
                    {
                        Weapon weapon = new Weapon();
                        weapon.Attacks = weaponAttacks;
                        weapon.Range = weaponRange;
                        if (iModel == ModelCount) iModel = 0;
                        Models[iModel].Weapons.Add(weapon);
                    }

                    //weapon.Parse(strWeapon);
                }

                Console.WriteLine("-- PRASE RESULT ---");
                Console.WriteLine(Name + " [" + ModelCount + "] Q" + Quality + "+ D" + Defense + "+");
            }
        }

        private List<string> SplitWeapons(string baseStr)
        {
            List<string> list = new List<string>();
            bool breakesMode = false;
            string currentWeaponStr = "";
            foreach(char c in baseStr)
            {
                currentWeaponStr += c;
                if(c == '(')
                {
                    breakesMode = true;
                    continue;
                }
                if(breakesMode && c == ')')
                {
                    breakesMode = false;
                    continue;
                }
                if(!breakesMode && c == ',')
                {
                    list.Add(currentWeaponStr);
                    currentWeaponStr = ""; // next weapon
                }
            }
            if( currentWeaponStr.Length > 0 )
            {
                list.Add(currentWeaponStr);
            }
            return list;
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

        internal string Status()
        {
            List<int> modelToughness = new List<int>();
            Models.ForEach(x => modelToughness.Add(x.CurrentToughness));
            return Name + " Models (" + string.Join(", ", modelToughness) + ")";
        }

        internal int AliveModels()
        {
            int alive = 0;
            foreach(var model in Models)
            {
                if(model.IsAlive()) alive++;
            }
            return alive;
        }

        internal void Reset()
        {
            foreach (var model in Models)
            {
                model.CurrentToughness = model.MaxToughness;
            }
        }
    }
}
