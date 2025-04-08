using System.Collections.Generic;
using UnityEngine;

namespace Configs 
{
    [CreateAssetMenu(menuName="Configs/ItemsConfig", fileName = "ItemsConfig")]
    public class ItemsConfig : ScriptableObject
    {
        [SerializeField] private List<ItemData> _itemDatas;
        private Dictionary<string, ItemData> _itemDataDictionary;
        
        public ItemData GetItemData(string id)
        {
            if (_itemDataDictionary == null || _itemDataDictionary.Count == 0)
                FillItemDataMap();

            return _itemDataDictionary[id];
        }

        private void FillItemDataMap()
        {
            _itemDataDictionary = new Dictionary<string, ItemData>();
            foreach (var itemData in _itemDatas)
                _itemDataDictionary.TryAdd(itemData.Id, itemData);
        }
    }
}