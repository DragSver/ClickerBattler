using UnityEngine;

namespace ClickRPG
{
    [CreateAssetMenu(menuName = "Configs/EnemiesConfig", fileName = "EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        public Enemy EnemyPrefab;
        public EnemyData[] Enemies;
    }
}