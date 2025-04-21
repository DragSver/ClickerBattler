using System.Collections.Generic;

namespace Global.SaveSystem.SavableObjects
{
    public class Stats : ISavable
    {
        public int KillsCount;
        public int DeathCount;
        public int ClickCount;
        public int BossKillsCount;
        public int NoneElementKillsCount;
        public int MoonKillsCount;
        public int SunKillsCount;
        public int WaterKillsCount;
        public int BloodKillsCount;
        public Dictionary<string, int> EnemiesKillCount = new ();
    }
}