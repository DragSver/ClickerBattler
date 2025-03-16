using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG
{
    public class EnemyController : MonoBehaviour
    {
        public event UnityAction OnDead;
        public event UnityAction<float> OnDamaged;
        
        [SerializeField] private Transform _enemyContainer;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private HealthBar _healthBarPrefab;
        
        private EnemyData _currentEnemyData;
        private Enemy _currentEnemy;

        private HealthBar _currentHealthBar;

        public void Init()
        {
            SpawnEnemy();
        }

        public void SpawnEnemy()
        {
            _currentEnemyData = _enemiesConfig.Enemies[^1];
            _currentEnemy = Instantiate(_enemiesConfig.EnemyPrefab, _enemyContainer);
            _currentEnemy.Init(_currentEnemyData);
            _currentEnemy.OnDead += () => OnDead?.Invoke();

            _currentHealthBar = Instantiate(_healthBarPrefab, _enemyContainer);
            _currentHealthBar.SetMaxValue(_currentEnemyData.Health);
        }

        public void DamageCurrentEnemy(float damage)
        {
            _currentEnemy.DoDamage(damage);
            
            _currentHealthBar.DecreaseValue(damage);
        }
    }
}