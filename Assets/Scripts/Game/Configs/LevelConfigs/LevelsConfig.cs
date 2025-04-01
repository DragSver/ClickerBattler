using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs.LevelConfigs 
{
    [CreateAssetMenu(menuName="Configs/LevelsConfig", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject 
    {
        public List<LocationLevelData> Locations;
        
        public LevelData GetLevel(int location, int level) {
            foreach (var locationLevelData in Locations)
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
    }
}