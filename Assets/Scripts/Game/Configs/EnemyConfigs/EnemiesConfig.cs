using UnityEngine;

namespace ClickRPG
{
    [CreateAssetMenu(menuName = "Configs/EnemiesConfig", fileName = "EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        public Enemy EnemyPrefab;
        public EnemyViewData[] Enemies;

        public EnemyViewData GetEnemy(string id)
        {
            foreach (var enemyData in Enemies)
            {
                if (enemyData.Id == id) return enemyData;
            }
            
            Debug.LogError($"Не найден враг с id {id}");
            return default;
        }
    }
}