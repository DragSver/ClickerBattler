using System;
using System.Collections.Generic;
using Meta.Locations;

namespace Game.Configs.LevelConfigs
{
    [Serializable]
    public struct LocationLevelData 
    {
        public int Location;
        public List<LevelData> Levels;
        public ProgressState State;
    }
}