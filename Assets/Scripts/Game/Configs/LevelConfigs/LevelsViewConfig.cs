using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs.LevelConfigs 
{
    [CreateAssetMenu(menuName="Configs/LevelsViewConfig", fileName = "LevelsViewConfig")]
    public class LevelsViewConfig : ScriptableObject
    {
        public List<LocationViewData> LocationsViews;
        public Dictionary<int, LocationViewData> LocationsViewsDictionary;

        public void Init()
        {
            LocationsViewsDictionary = new Dictionary<int, LocationViewData>();
            foreach (var locationViewData in LocationsViews)
                LocationsViewsDictionary.Add(locationViewData.Id, locationViewData);
        }
        
        public LocationViewData GetLocationViewData(int locationId) => LocationsViewsDictionary[locationId];
    }
}