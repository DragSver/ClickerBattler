using System;

namespace Game.Configs.Levels.Data 
{
    [Serializable]
    public struct EnemySpawnData 
    {
        public string Id;
        public int Hp;
        public bool IsBoss;
        public float BossTime;
    }
}