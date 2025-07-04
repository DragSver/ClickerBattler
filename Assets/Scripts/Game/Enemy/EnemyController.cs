using System;
using System.Collections.Generic;
using System.Linq;
using Configs.EnemyConfigs;
using Datas.Game.EnemiesData;
using Datas.Global;
using Game.Timer;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Game.Enemy
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
        
        [SerializeField] private EnemyView _bossEnemyView;

        private List<Enemy> _currentEnemies;
        private EnemyView[] _enemyViews;

        private LevelData _levelData;
        private SaveSystem _saveSystem;
        private Stats _stats;
        private int _currentEnemyWiveIndex;

        private TimerController _timerController;

        public void Init(TimerController timerController, Stats stats, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _stats = stats;
            _enemiesConfig.Init();

            _currentEnemies = new List<Enemy>();
            _enemyViews = new[]
                { _centralEnemy, _leftForwardEnemy, _leftBackEnemy, _rightForwardEnemy, _rightBackEnemy, _bossEnemyView };
            foreach (var enemyView in _enemyViews) enemyView.Init();
            
            _timerController = timerController;
        }
        
        public void StartLevel(LevelData levelData)
        {
            _levelData = levelData;
            _currentEnemyWiveIndex = -1;
            
            SpawnWiveEnemy();
        }
        private void SpawnWiveEnemy()
        {
            _currentEnemies.Clear();
            foreach (var enemyView in _enemyViews) enemyView.ClearEnemy();
            
            _currentEnemyWiveIndex++;
            if (_currentEnemyWiveIndex >= _levelData.EnemiesWives.Count)
            {
                EndLevel(true);
                return;
            }

            
            var currentEnemyWiveSpawnData = _levelData.EnemiesWives[_currentEnemyWiveIndex];

            if (currentEnemyWiveSpawnData.IsBoss)
                InitBossEnemies(currentEnemyWiveSpawnData);
            else
            {
                if (currentEnemyWiveSpawnData.Enemies.Count%2 == 0)
                    InitEvenEnemies(currentEnemyWiveSpawnData.Enemies);
                else 
                    InitUnevenEnemies(currentEnemyWiveSpawnData.Enemies);
            }

            InitTimer(currentEnemyWiveSpawnData.Time);
        }

        private void InitBossEnemies(EnemiesWiveData currentEnemyWiveSpawnData)
        {
            var bossInit = false;
            for (var index = 0; index < currentEnemyWiveSpawnData.Enemies.Count; index++)
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
        private void InitUnevenEnemies(List<EnemySpawnData> enemySpawnDatas)
        {
            for (var i = 0; i < enemySpawnDatas.Count; i++)
            {
                var enemySpawnData = enemySpawnDatas[i];
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
        private void InitEvenEnemies(List<EnemySpawnData> enemySpawnDatas)
        {
            for (var i = 0; i < enemySpawnDatas.Count; i++)
            {
                var enemySpawnData = enemySpawnDatas[i];
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
            newEnemy.OnDead += e => { OnDead(e, enemySpawnData); }; 
            _currentEnemies.Add(newEnemy);
        }
        
        private void InitTimer(float time)
        {
            if (time != 0)
            {
                _timerController.SetActive(true);
                _timerController.SetMaxTime(time);
                _timerController.OnTimerEnd += () => EndLevel(false);
                _timerController.Play();
            }
            else _timerController.SetActive(false);
        }

        public void DamageEnemy(Enemy enemy, Elements element, float damage)
        {
            enemy.DoDamage(element, damage);
        }
        public void DamageRandomEnemy(Elements element, float damage)
        {
            DamageEnemy(_currentEnemies[Random.Range(0, _currentEnemies.Count)], element, damage);
        }
        
        private void OnDamaged(float damage, ElementsInfluence influence)
        {
            
        }
        private void OnDead(ElementsInfluence influence, EnemySpawnData enemySpawnData)
        {
            AddKillStats(enemySpawnData);
            
            if (_currentEnemies.Any(enemy => enemy.GetHealth() != 0)) return;
            
            _timerController.Stop();
            SpawnWiveEnemy();
        }
        private void AddKillStats(EnemySpawnData enemySpawnData)
        {
            _stats.KillsCount++;
            if (enemySpawnData.IsBoss) _stats.BossKillsCount++;
            if (_stats.EnemiesKillCount.TryGetValue(enemySpawnData.Id, out var killCount))
                _stats.EnemiesKillCount[enemySpawnData.Id] = killCount + 1;
            else
                _stats.EnemiesKillCount.TryAdd(enemySpawnData.Id, 1);
            var enemy = _enemiesConfig.GetEnemy(enemySpawnData.Id);
            switch (enemy.Element)
            {
                case Elements.None:
                    _stats.NoneElementKillsCount++;
                    break;
                case Elements.Blood:
                    _stats.BloodKillsCount++;
                    break;
                case Elements.Water:
                    _stats.WaterKillsCount++;
                    break;
                case Elements.Sun:
                    _stats.SunKillsCount++;
                    break;
                case Elements.Moon:
                    _stats.MoonKillsCount++;
                    break;
            }
        }
        
        private void EndLevel(bool win)
        {
            _timerController.Stop();
            
            if (!win) _stats.DeathCount++;
            
            _saveSystem.SaveData(SavableObjectType.Stats);
            OnLevelComplete?.Invoke(win);
        }
    }
}