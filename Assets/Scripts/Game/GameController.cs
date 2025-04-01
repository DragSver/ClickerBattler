using ClickRPG.CriticalHit;
using ClickRPG.Meta;
using ClickRPG.SceneManagement;
using Game.Configs.Levels;
using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using System.Linq;
using ClickRPG;
using ClickRPG.SceneManagment;
using Game.Configs.LevelConfigs;
using Game.CriticalHit;
using Kolobrod.Game.Enemy;
using UnityEngine;

namespace Game {
    public class GameController : EntryPoint
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;
        
        [Header("GameScreen")]
        [SerializeField] private Timer.Timer _timer;
        [SerializeField] private LevelsConfig _levels;
        [SerializeField] private LevelsViewConfig _levelsViewConfig;
        [SerializeField] private LocationViewController _locationViewController;
        [SerializeField] private CriticalHitController _criticalHitController;
        [SerializeField] private AttackButtonController _attackButtonController;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private LevelData _levelData;
        
        private GameEnterParams _gameEnterParams;
        private SaveSystem _saveSystem;
        private AudioManager _audioManager;
        
        private Progress _progress;

        private const string SCENE_LOADER_TAG = "SceneLoader";

        
        public override void Run(SceneEnterParams enterParams)
        {
            _saveSystem = FindFirstObjectByType<SaveSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _audioManager.Play(AudioNames.Audio_Game_BG, false);
            
            if (enterParams is not GameEnterParams)
            {
                Debug.LogError("Неправильная передача параметров сцены");
                return;
            }
            var gameEnterParams = (GameEnterParams)enterParams;
            _gameEnterParams = gameEnterParams;
            
            _enemyController.Init(_timer);
            _levelsViewConfig.Init();

            StartLevel();
        }

        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            
            _levelData = _levels.GetLevel(_gameEnterParams.Location, _gameEnterParams.Level);

            var locationView = _levelsViewConfig.GetLocationViewData(_levelData.Location);
            _locationViewController.SetLocationView(locationView);
            _attackButtonController = _locationViewController.AttackButtonController;
            _attackButtonController.Init();
            _criticalHitController.Init(_attackButtonController.GetComponent<RectTransform>());
            _attackButtonController.OnClick += AttackClick;
            
            _enemyController.StartLevel(_levelData);
            _enemyController.OnLevelComplete += EndLevel;
            _criticalHitController.StartGenerateCriticalPoint();
        }

        private void EndLevel(bool levelPassed)
        {
            _timer.Stop();
            _criticalHitController.StopGenerateCriticalPoint();
            
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (levelPassed)
            {
                _progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
                if (_gameEnterParams.Location == _progress.CurrentLocation &&
                    _gameEnterParams.Level == _progress.CurrentLevel)
                {
                    var maxLevel = _levels.GetMaxLevelOnLocation(_progress.CurrentLocation);
                    if (_progress.CurrentLevel >= maxLevel)
                    {
                        _progress.CurrentLevel = 1;
                        _progress.CurrentLocation++;
                    }
                    else _progress.CurrentLevel++;
                    _saveSystem.SaveData(SavableObjectType.Progress);
                }
                
                var currentTime = _maxLevelTime - _timer.CurrentTime;
                var bestTime = GameStats.SaveBestTime(currentTime);
                var totalKills = GameStats.AddKills();
                
                var victoryData = _victoryScreenData;
                var totalKills = GameStats.GetKills();
                
                var maxLevelTime = 0f;
                foreach (var levelDataEnemiesWive in _levelData.EnemiesWives.Where(levelDataEnemiesWive => levelDataEnemiesWive.Time > 0)) 
                    maxLevelTime = levelDataEnemiesWive.Time;
                if (maxLevelTime > 0)
                {
                    var currentTime = maxLevelTime - _timer.CurrentTime;
                    var bestTime = GameStats.SaveBestTime(currentTime);
                    
                    victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                    victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
                }
                victoryData.StatisticText = victoryData.StatisticText.Replace("N", totalKills.ToString());
                
                _endLevelScreenController.OnContinueGameClick += NextLevel;

                _endLevelScreenController.CallEndLevelScreen(victoryData);
            }
            else
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.OnContinueGameClick += RestartLevel;
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
            }
        }

        private void DamageEnemy(Enemy enemy, Elements element, float damage) =>
            _enemyController.DamageCurrentEnemy(enemy, element, damage);

        private void RestartLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(_gameEnterParams);
            _endLevelScreenController.OnContinueGameClick -= RestartLevel;
        }
        private void NextLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(new GameEnterParams(level: _progress.CurrentLevel, location: _progress.CurrentLocation));
            _endLevelScreenController.OnContinueGameClick -= NextLevel;
        }
        
        private void LoadMetaScene()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadMetaScene();
        }

        private void AttackClick(Enemy enemy, Elements element, float damage)
        {
            var multiplierDamage = _criticalHitController.GetDamageMultiplierPointerPosition(damage);
            DamageEnemy(enemy, element, damage);
        }
    }
}
