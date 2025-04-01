using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs.LevelConfigs 
{
    [Serializable]
    public struct LevelData 
    {
        public int Location;
        public int LevelNumber;
        public bool IsComleted;
        public bool ExtraLevel;
        public List<EnemiesWiveData> EnemiesWives;
        public List<CollectedItemsData> Rewards;
    }
}