namespace OprBattleSim.Model
{
    public class Battle
    {
        public String Unit1String = "++ Orc Marauders [200pts] ++\r\n\r\nBoss Mob [5] Q3+ D5+ | 200pts | Bad Shot, Furious, Tough(3)\r\n5x CCWs (A3), 5x Pistols (12\", A1)\r\n";
        public String Unit2String = "++ Orc Marauders [200pts] ++\r\n\r\nBoss Mob [5] Q3+ D5+ | 200pts | Bad Shot, Furious, Tough(3)\r\n5x CCWs (A3), 5x Pistols (12\", A1)\r\n";
        public String Result = "";

        private Unit Unit1 = new Unit();
        private Unit Unit2 = new Unit();
         
        public void Start()
        {
            Result = Unit1String + Unit2String;
            Unit1.Parse(Unit1String);
            Unit2.Parse(Unit2String);

        }
    }
}
