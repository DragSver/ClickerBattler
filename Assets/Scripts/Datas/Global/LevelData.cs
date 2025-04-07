using System;
using System.Collections.Generic;
using Datas.Game.EnemiesData;

namespace Datas.Global 
{
    [Serializable]
    public struct LevelData 
    {
        public int Location;
        public int LevelNumber;
        public bool ExtraLevel;
        public List<EnemiesWiveData> EnemiesWives;
        public List<CollectedItemsData> Rewards;
    }
}