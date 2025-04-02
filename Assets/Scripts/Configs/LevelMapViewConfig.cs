using System.Collections.Generic;
using Datas.Meta;
using UnityEngine;

namespace Configs 
{
    [CreateAssetMenu(menuName="Configs/LevelMapViewConfig", fileName = "LevelMapViewConfig")]
    public class LevelMapViewConfig : ScriptableObject
    {
        [SerializeField] private List<LevelMapViewData> _levelMapViewDatas;
        private Dictionary<int, LevelMapViewData> _levelMapViewDictionary;

        public void Init()
        {
            _levelMapViewDictionary = new Dictionary<int, LevelMapViewData>();
            foreach (var levelMapViewData in _levelMapViewDatas)
                _levelMapViewDictionary.Add(levelMapViewData.Id, levelMapViewData);
        }
        
        public LevelMapViewData GetLevelMapViewData(int locationId) => _levelMapViewDictionary[locationId];
    }
}