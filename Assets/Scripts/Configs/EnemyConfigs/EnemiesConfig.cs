using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;

namespace Configs.EnemyConfigs
{
    [CreateAssetMenu(menuName = "Configs/EnemiesConfig", fileName = "EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        [SerializeField] private EnemyViewData[] _enemies;
        private Dictionary<string, EnemyViewData> _enemiesDict;

        public void Init()
        {
            _enemiesDict = new Dictionary<string, EnemyViewData>();
            foreach (var enemyViewData in _enemies)
                _enemiesDict.Add(enemyViewData.Id, enemyViewData);
        }
        
        public EnemyViewData GetEnemy(string id) => _enemiesDict[id];
    }
}