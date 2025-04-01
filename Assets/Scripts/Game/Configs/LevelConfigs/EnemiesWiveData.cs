using System;
using System.Linq;

namespace Game.Configs.LevelConfigs
{
    [Serializable]
    public struct EnemiesWiveData
    {
        public float Time;
        public EnemySpawnData[] Enemies;
        
        public bool IsBoss => Enemies.Any(e => e.IsBoss);
    }
}