using System;

namespace Game.Configs.LevelConfigs 
{
    [Serializable]
    public struct EnemySpawnData 
    {
        public string Id;
        public int Hp;
        public bool IsBoss;
    }
}