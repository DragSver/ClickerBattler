using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;

namespace Game.Configs.EnemyConfigs
{
    [CreateAssetMenu(menuName = "Configs/EnemiesConfig", fileName = "EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        public EnemyViewData[] Enemies;
        public Dictionary<string, EnemyViewData> EnemiesDict;

        public void Init()
        {
            EnemiesDict = new Dictionary<string, EnemyViewData>();
            foreach (var enemyViewData in Enemies)
                EnemiesDict.Add(enemyViewData.Id, enemyViewData);
        }
        
        public EnemyViewData GetEnemy(string id) => EnemiesDict[id];
    }
}