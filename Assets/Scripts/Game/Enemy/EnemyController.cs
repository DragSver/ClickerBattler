using Game.Configs.Levels.Data;
using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG
{
    public class EnemyController : MonoBehaviour
    {
        public event UnityAction<float> OnDamaged;
        public event UnityAction<bool> OnLevelPassed;
        
        [SerializeField] private Transform _enemyContainer;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private HealthBar _healthBarPrefab;

        private Enemy _currentEnemy;
        private int _currentEnemyIndex;

        private LevelData _levelData;

        private HealthBar _healthBar;
        private Timer _timer;

        private HealthBar _currentHealthBar;

        public void Init(Timer timer)
        {
            _timer = timer;
            _currentHealthBar = Instantiate(_healthBarPrefab, _enemyContainer);
        }

        public void StartLevel(LevelData levelData)
        {
            _levelData = levelData;
            _currentEnemyIndex = -1;
            
            if (_currentEnemy == null)
            {
                _currentEnemy = Instantiate(_enemiesConfig.EnemyPrefab, _enemyContainer);
                _currentEnemy.OnDead += OnDead;
                _currentEnemy.OnDamaged += _currentHealthBar.DecreaseValue;
            }
            
            SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            _currentEnemyIndex++;
            if (_currentEnemyIndex >= _levelData.Enemies.Count)
            {
                _timer.Stop();
                OnLevelPassed?.Invoke(true);
                return;
            }
            
            var currentEnemySpawnData = _levelData.Enemies[_currentEnemyIndex];
            InitHpBar(currentEnemySpawnData.Hp);

            if (currentEnemySpawnData.IsBoss)
            {
                _timer.gameObject.SetActive(true);
                InitTimer(currentEnemySpawnData.BossTime);
            }
            else _timer.gameObject.SetActive(false);

            var currentEnemyViewData = _enemiesConfig.GetEnemy(currentEnemySpawnData.Id);
            _currentEnemy.Init(currentEnemyViewData.Sprite, currentEnemySpawnData.Hp);

            _timer.Play();
        }

        private void InitHpBar(int health)
        {
            _currentHealthBar.SetMaxValue(health);
        }
        private void InitTimer(float time)
        {
            _timer.SetMaxTime(time);
            _timer.OnTimerEnd += () => OnLevelPassed?.Invoke(false);
        }
        
        public void DamageCurrentEnemy(float damage)
        {
            _currentEnemy.DoDamage(damage);
        }

        private void OnDead()
        {
            _timer.Stop();
            SpawnEnemy();
        }
    }
}