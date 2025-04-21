using System.Collections.Generic;
using System.Linq;
using Datas.Global;
using UnityEngine;

namespace Configs.LevelConfigs 
{
    [CreateAssetMenu(menuName="Configs/LevelsConfig", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        public List<LocationLevelData> Locations => _locations;
        [SerializeField] private List<LocationLevelData> _locations;
        
        public LevelData GetLevel(int location, int level) {
            foreach (var locationLevelData in _locations)
            {
                if (locationLevelData.Location != location) continue;
                foreach (var levelData in locationLevelData.Levels)
                {
                    if (levelData.LevelNumber == level)
                        return levelData;
                }
            }
            
            // Debug.LogError($"Not found Level data for location {location} and level {level}");
            return default;
        }

        public LocationLevelData GetLocationLevelData(int location)
        {
            foreach (var locationLevelData in _locations.Where(locationLevelData => locationLevelData.Location == location)) return locationLevelData;
            
            // Debug.LogError($"Not found Level data for location {location}");
            return default;
        }

        public void Clear()
        {
            _locations = new List<LocationLevelData>();
        }
        
        public int GetCountMainLevelOnLocation(int location)
        {
            foreach (var locationLevelData in _locations)
            {
                if (locationLevelData.Location == location)
                    return locationLevelData.Levels.FindAll(data => !data.ExtraLevel).Count;
            }
            return default;
        }
    }
}