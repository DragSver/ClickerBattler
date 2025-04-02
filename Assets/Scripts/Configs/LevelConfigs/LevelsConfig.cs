using System.Collections.Generic;
using Datas.Global;
using UnityEngine;

namespace Configs.LevelConfigs 
{
    [CreateAssetMenu(menuName="Configs/LevelsConfig", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject 
    {
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
            
            Debug.LogError($"Not found Level data for location {location} and level {level}");
            return default;
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