using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace OprBattleSim.Model
{
    public class Battle
    {
        // Unit/Output UI bindings - This should be seperated in a binding class later on
        public String Unit1String = "++ Orc Marauders [200pts] ++\r\n\r\nBoss Mob [5] Q4+ D5+ | 200pts | Bad Shot, Furious, Tough(3)\r\n5x CCWs (A3), 5x Pistols (12\", A1)\r\n";
        public String Unit2String = "++ Orc Marauders [200pts] ++\r\n\r\nBoss Mob [5] Q4+ D5+ | 200pts | Bad Shot, Furious, Tough(3)\r\n5x CCWs (A3), 5x Pistols (12\", A1)\r\n";
        public String Result = "";
        public String Distance = "0";
        
        public List<String> ResultLog = new List<String>();
        private Unit Unit1 = new Unit();
        private Unit Unit2 = new Unit();

        public void Start()
        {
            ResultLog.Clear();
            Result = Unit1String + Unit2String;
            Unit1 = new Unit();
            Unit2 = new Unit();
            Unit1.Parse(Unit1String);
            Unit2.Parse(Unit2String);
            int fightCount = 100;
            int winsUnit1 = Fight(Unit1, Unit2, fightCount);
            int winsUnit2 = Fight(Unit2, Unit1, fightCount);

            Log(@$"Unit 1 won { winsUnit1 + (fightCount - winsUnit2) } times out of {fightCount * 2} fights.");
            Result = String.Join(ControlChars.NewLine, ResultLog);
        }

        private int Fight(Unit unit1, Unit unit2, int rounds = 100)
        {
            Log(unit1.Name + " attacks first for " + rounds + " fights");
            int unit1Won = 0;
            Dictionary<int, int> countByToughnessUnit1 = new Dictionary<int, int>();
            Dictionary<int, int> countByToughnessUnit2 = new Dictionary<int, int>();
            for (int i = 0; i < rounds; i++)
            {
                int remainingModelsUnit1 = unit1.Models.Count;
                int remainingModelsUnit2 = unit2.Models.Count;

                // we fight till one unit is dead
                while (unit1.IsAlive() && unit2.IsAlive())
                {
                    unit1.Attack(unit2);
                    if (unit2.IsAlive())
                    {
                        unit2.Attack(unit1);
                    }
                    Log("1: " + unit1.Status());
                    Log("2: " + unit2.Status());
                }
                remainingModelsUnit1 = unit1.AliveModels();
                remainingModelsUnit2 = unit2.AliveModels();
                AddCountByToughness(countByToughnessUnit1, remainingModelsUnit1);
                AddCountByToughness(countByToughnessUnit2, remainingModelsUnit2);
                Log("Round over: " + remainingModelsUnit1 + " vs " + remainingModelsUnit2);
                if (remainingModelsUnit2 < remainingModelsUnit1)
                {
                    unit1Won++;
                }
                Log(" ");
                unit1.Reset();
                unit2.Reset();
            }
            Log(" ");
            Log("Unit1 won " + unit1Won + " rounds out of " + rounds);
            Log(" ");
            Log(PrintToughnessDic(countByToughnessUnit1));
            Log(PrintToughnessDic(countByToughnessUnit2));
            return unit1Won;
        }

        private string PrintToughnessDic(Dictionary<int, int> countByToughnessUnit1)
        {
            string str = "";
            foreach(KeyValuePair<int, int> kvp in countByToughnessUnit1.OrderBy(x => x.Key))
            {
                str += kvp.Key + "=" + kvp.Value + ", ";
            }
            return str;
        }

        private void AddCountByToughness(Dictionary<int, int> countByToughnessUnit1, int remainingModelsUnit1)
        {
            if(!countByToughnessUnit1.ContainsKey(remainingModelsUnit1))
            {
                countByToughnessUnit1.Add(remainingModelsUnit1, 0);
            }
            countByToughnessUnit1[remainingModelsUnit1]++;
        }

        private void Log(string v)
        {
            ResultLog.Add(v);
        }
    }
}
