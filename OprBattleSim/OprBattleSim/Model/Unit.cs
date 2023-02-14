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
        public List<Weapon> Weapons = new List<Weapon>();

        private Regex nameRegex = new Regex("(.*) \\[");
        private Regex modelCountRegex = new Regex("\\[([0-9]+)\\]");
        private Regex qualityRegex = new Regex(" Q([0-9]{1})\\+ ");
        private Regex defenseRegex = new Regex("D([0-9]{1})\\+");

        internal void Parse(string unit2String)
        {
            // split the units by doulbe linebreak
            foreach(String strUnit in unit2String.Split(ControlChars.NewLine + ControlChars.NewLine))
            {                
                List<String> lines = strUnit.Split(ControlChars.NewLine).ToList();
                if (lines.Count < 2)
                {
                    continue;
                }
                String line1 = lines[0];
                String line2 = lines[1];
                List<String> line1parts = lines[0].Split(" | ").ToList();
                String baseStatsStr = line1parts[0];
                Name = nameRegex.Matches(baseStatsStr)[0].Groups[1].Value;
                ModelCount = Int32.Parse( modelCountRegex.Matches(baseStatsStr)[0].Groups[1].Value);
                Quality = Int32.Parse(qualityRegex.Matches(baseStatsStr)[0].Groups[1].Value);
                var test = defenseRegex.Matches(baseStatsStr);
                Defense = Int32.Parse(defenseRegex.Matches(baseStatsStr)[0].Groups[1].Value);

                String pointCostsStr = line1parts[1];
                String skillsStr = line1parts[2];
                List<String> weaponsStr = lines[0].Split(", ").ToList();

                foreach(String strWeapon in weaponsStr)
                {
                    Weapon weapon= new Weapon();
                    //weapon.Parse(strWeapon);
                }


                Console.WriteLine("-- PRASE RESULT ---");
                Console.WriteLine(Name + " [" + ModelCount + "] Q" + Quality + "+ D" + Defense + "+" );
            }
        }
    }
}
