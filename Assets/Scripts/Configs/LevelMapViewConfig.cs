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


        public LevelMapViewData GetLevelMapViewData(int locationId)
        {
            if (_levelMapViewDictionary == null || _levelMapViewDictionary.Count == 0)
                FillDictionary();
            
            return _levelMapViewDictionary[locationId];  
        } 
        private void FillDictionary()
        {
            _levelMapViewDictionary = new Dictionary<int, LevelMapViewData>();
            foreach (var levelMapViewData in _levelMapViewDatas)
                _levelMapViewDictionary.Add(levelMapViewData.Id, levelMapViewData);
        }
    }
}
