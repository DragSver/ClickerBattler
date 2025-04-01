using System;
using System.Collections.Generic;
using ClickRPG.Meta.Locations;

namespace Game.Configs.LevelConfigs
{
    [Serializable]
    public struct LocationLevelData 
    {
        public int Location;
        public List<LevelData> Levels;
        public PinType Type;
    }
}