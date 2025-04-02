using System;
using System.Linq;

namespace Datas.Game.EnemiesData
{
    [Serializable]
    public struct EnemiesWiveData
    {
        public float Time;
        public EnemySpawnData[] Enemies;
        
        public bool IsBoss => Enemies.Any(e => e.IsBoss);
    }
}