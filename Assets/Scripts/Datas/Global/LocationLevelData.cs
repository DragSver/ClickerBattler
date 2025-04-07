using System;
using System.Collections.Generic;
using Meta.Locations;

namespace Datas.Global
{
    [Serializable]
    public struct LocationLevelData 
    {
        public int Location;
        public List<LevelData> Levels;
    }
}