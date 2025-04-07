using System;

namespace Datas.Game.EnemiesData 
{
    [Serializable]
    public struct EnemySpawnData 
    {
        public string Id;
        public int Hp;
        public bool AutoHP;
        public bool IsBoss;
    }
}