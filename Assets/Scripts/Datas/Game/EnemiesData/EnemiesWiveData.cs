using System;
using System.Collections.Generic;
using System.Linq;

namespace Datas.Game.EnemiesData
{
    [Serializable]
    public struct EnemiesWiveData
    {
        public int Location;
        public int LevelNumber;
        public int Wive;
        public float Time;
        public List<EnemySpawnData> Enemies;
        
        public bool IsBoss => Enemies.Any(e => e.IsBoss);
    }
}