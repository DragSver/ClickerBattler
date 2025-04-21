using System;

namespace Datas.Game.EnemiesData 
{
    [Serializable]
    public struct EnemySpawnData 
    {
        public int Location;
        public int LevelNumber;
        public int Wive;
        
        public string Id;
        public int Hp;
        public bool AutoHP;
        public bool IsBoss;
    }
}