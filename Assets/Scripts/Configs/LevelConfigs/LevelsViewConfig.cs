using System.Collections.Generic;
using Datas.Game;
using UnityEngine;

namespace Configs.LevelConfigs 
{
    [CreateAssetMenu(menuName="Configs/LevelsViewConfig", fileName = "LevelsViewConfig")]
    public class LevelsViewConfig : ScriptableObject
    {
        [SerializeField] private List<LocationViewData> _locationsViews;
        private Dictionary<int, LocationViewData> _locationsViewsDictionary;

        public void Init()
        {
            _locationsViewsDictionary = new Dictionary<int, LocationViewData>();
            foreach (var locationViewData in _locationsViews)
                _locationsViewsDictionary.Add(locationViewData.Id, locationViewData);
        }
        
        public LocationViewData GetLocationViewData(int locationId) => _locationsViewsDictionary[locationId];
    }
}