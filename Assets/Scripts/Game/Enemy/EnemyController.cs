using System.Collections.Generic;
using System.Linq;
using ClickRPG;
using Game.Configs.EnemyConfigs;
using Game.Configs.LevelConfigs;
using Game.Timer;
using UnityEngine;
using UnityEngine.Events;

namespace Kolobrod.Game.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public event UnityAction<bool> OnLevelComplete;
        
        [SerializeField] private EnemiesConfig _enemiesConfig;

        [SerializeField] private EnemyView _centralEnemy;
        [SerializeField] private EnemyView _leftForwardEnemy;
        [SerializeField] private EnemyView _leftBackEnemy;
        [SerializeField] private EnemyView _rightForwardEnemy;
        [SerializeField] private EnemyView _rightBackEnemy;
        
        [SerializeField] private BossEnemyView _bossEnemyView;

        private List<Enemy> _currentEnemies = new List<Enemy>();

        private LevelData _levelData;
        private int _currentEnemyWiveIndex;

        private Timer _timer;

        public void Init(Timer timer)
        {
            _timer = timer;
            
            _centralEnemy.Init();
            _leftForwardEnemy.Init();
            _leftBackEnemy.Init();
            _rightBackEnemy.Init();
            _rightForwardEnemy.Init();
            
            _enemiesConfig.Init();
        }

        public void DamageCurrentEnemy(Enemy enemy, Elements element, float damage)
        {
            enemy.DoDamage(element, damage);
        }
        
        public void StartLevel(LevelData levelData)
        {
            _levelData = levelData;
            _currentEnemyWiveIndex = -1;
            
            SpawnWiveEnemy();
        }
        
        private void SpawnWiveEnemy()
        {
            _centralEnemy.ClearEnemy();
            _leftForwardEnemy.ClearEnemy();
            _leftBackEnemy.ClearEnemy();
            _rightBackEnemy.ClearEnemy();
            _rightForwardEnemy.ClearEnemy();
            
            _currentEnemyWiveIndex++;
            if (_currentEnemyWiveIndex >= _levelData.EnemiesWives.Count)
            {
                _timer.Stop();
                OnLevelComplete?.Invoke(true);
                return;
            }

            var currentEnemyWiveSpawnData = _levelData.EnemiesWives[_currentEnemyWiveIndex];

            if (currentEnemyWiveSpawnData.IsBoss)
                InitBossEnemies(currentEnemyWiveSpawnData);
            else
            {
                if (currentEnemyWiveSpawnData.Enemies.Count()/2 == 0)
                    InitOddEnemies(currentEnemyWiveSpawnData.Enemies);
                else InitEvenEnemies(currentEnemyWiveSpawnData.Enemies);
            }

            InitTimer(currentEnemyWiveSpawnData.Time);
        }

        private void InitBossEnemies(EnemiesWiveData currentEnemyWiveSpawnData)
        {
            var bossInit = false;
            for (var index = 0; index < currentEnemyWiveSpawnData.Enemies.Length; index++)
            {
                var enemySpawnData = currentEnemyWiveSpawnData.Enemies[index];

                EnemyView enemyView;
                if (enemySpawnData.IsBoss && !bossInit)
                {
                    enemyView = _bossEnemyView;
                    bossInit = true;
                }
                else
                {
                    var countInitSimpleEnemy = bossInit ? index - 1 : index;
                    switch (countInitSimpleEnemy)
                    {
                        case 0:
                            enemyView = _leftBackEnemy;
                            break;
                        case 1:
                            enemyView = _rightBackEnemy;
                            break;
                        case 2:
                            enemyView = _leftForwardEnemy;
                            break;
                        case 3:
                            enemyView = _rightForwardEnemy;
                            break;
                        default:
                            return;
                    }
                }

                if (enemyView == null) break;

                CreateNewEnemy(enemySpawnData, enemyView);
            }
        }
        private void InitOddEnemies(EnemySpawnData[] enemySpawnDatas)
        {
            for (var i = 0; i < enemySpawnDatas.Length; i++)
            {
                var enemySpawnData = enemySpawnDatas[0];
                EnemyView enemyView;

                switch (i)
                {
                    case 0:
                        enemyView = _centralEnemy;
                        break;
                    case 1:
                        enemyView = _leftBackEnemy;
                        break;
                    case 2:
                        enemyView = _rightBackEnemy;
                        break;
                    case 3:
                        enemyView = _leftForwardEnemy;
                        break;
                    case 4:
                        enemyView = _rightForwardEnemy;
                        break;
                    default:
                        return;
                }

                CreateNewEnemy(enemySpawnData, enemyView);
            }
        }
        private void InitEvenEnemies(EnemySpawnData[] enemySpawnDatas)
        {
            for (var i = 0; i < enemySpawnDatas.Length; i++)
            {
                var enemySpawnData = enemySpawnDatas[0];
                EnemyView enemyView;

                switch (i)
                {
                    case 0:
                        enemyView = _leftForwardEnemy;
                        break;
                    case 1:
                        enemyView = _rightForwardEnemy;
                        break;
                    case 2:
                        enemyView = _leftBackEnemy;
                        break;
                    case 3:
                        enemyView = _rightBackEnemy;
                        break;
                    default:
                        return;
                }

                CreateNewEnemy(enemySpawnData, enemyView);
            }
        }
        private void CreateNewEnemy(EnemySpawnData enemySpawnData, EnemyView enemyView)
        {
            var newEnemy = new Enemy(enemySpawnData.Hp, enemyView, _enemiesConfig.GetEnemy(enemySpawnData.Id));
            newEnemy.OnDamaged += OnDamaged;
            newEnemy.OnDead += OnDead;
            _currentEnemies.Add(newEnemy);
        }
        
        private void InitTimer(float time)
        {
            if (time != 0)
            {
                _timer.gameObject.SetActive(true);
                _timer.SetMaxTime(time);
                _timer.OnTimerEnd += () => OnLevelComplete?.Invoke(false);
                _timer.Play();
            }
            else _timer.gameObject.SetActive(false);
        }

        private void OnDamaged(float damage)
        {
            
        }
        private void OnDead()
        {
            GameStats.AddKills();
            if (_currentEnemies.Any(enemy => enemy.GetHealth() != 0)) return;
            
            _timer.Stop();
            SpawnWiveEnemy();
        }
    }
}