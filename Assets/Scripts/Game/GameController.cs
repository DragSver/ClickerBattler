using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using System.Linq;
using Configs.LevelConfigs;
using Datas.Game;
using Datas.Global;
using Game.AttackButtons;
using Game.CriticalHit;
using Game.Enemy;
using Game.Timer;
using SceneManegement;
using SceneManegement.EnterParams;
using UnityEngine;

namespace Game {
    public class GameController : EntryPoint
    {
        [Header("Game")]
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private TimerController _timerController;
        [SerializeField] private AttackButtonController _attackButtonController;
        [SerializeField] private CriticalHitController _criticalHitController;

        [Header("UI")] 
        [SerializeField] private GameObject _pausePopup;
        [SerializeField] private LocationViewController _locationViewController;
        
        [Header("Configs")]
        [SerializeField] private LevelsConfig _levels;
        [SerializeField] private LevelsViewConfig _levelsViewConfig;

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
            _gameEnterParams = ReceiveGameEnterParams(enterParams);
            
            
            _enemyController.Init(_timerController);
            _levelsViewConfig.Init();

            StartLevel();
        }


        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            
            _levelData = _levels.GetLevel(_gameEnterParams.Location, _gameEnterParams.Level);

            var locationView = _levelsViewConfig.GetLocationViewData(_levelData.Location);
            _locationViewController.ClearLocationView();
            _locationViewController.SetLocationView(locationView,
                _gameEnterParams.Level, _levels.GetCountMainLevelOnLocation(_gameEnterParams.Location),
                0, LoadMetaScene, null, () =>
                {
                    _timerController.SwitchPause();
                    _pausePopup.SetActive(!_timerController.IsPlaying);
                });
            _attackButtonController = _locationViewController.AttackButtonController;
            _attackButtonController.Init(_locationViewController.Canvas);
            _criticalHitController.Init(_attackButtonController.GetComponent<RectTransform>());
            _attackButtonController.OnClick += AttackClick;
            
            _enemyController.StartLevel(_levelData);
            _enemyController.OnLevelComplete += EndLevel;
            _criticalHitController.StartGenerateCriticalPoint();
        }

        private void EndLevel(bool levelPassed)
        {
            _attackButtonController.Unsubscribe();
            _timerController.Stop();
            _criticalHitController.StopGenerateCriticalPoint();
            
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (levelPassed)
            {
                TrySaveEndLevelData();
                
                _endLevelScreenController.OnContinueGameClick += LoadNextLevel;
                _endLevelScreenController.CallEndLevelScreen(EditVictoryData(), LoadNextLevel, LoadMetaScene, true);
            }
            else
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.FirstLabel = loseData.FirstLabel.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.OnContinueGameClick += LoadRestartLevel;
                
                _endLevelScreenController.CallEndLevelScreen(loseData, LoadRestartLevel, LoadMetaScene, false);
            }
        }
        
        private void AttackClick(Enemy.Enemy enemy, Elements element, float damage)
        {
            var multiplierDamage = _criticalHitController.GetDamageMultiplierPointerPosition(damage);
            DamageEnemy(enemy, element, multiplierDamage);
        }
        private void DamageEnemy(Enemy.Enemy enemy, Elements element, float damage) =>
            _enemyController.DamageEnemy(enemy, element, damage);

        private void LoadRestartLevel()
        {
            _endLevelScreenController.OnContinueGameClick -= LoadRestartLevel;
            
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(_gameEnterParams);
        }
        private void LoadNextLevel()
        {
            _endLevelScreenController.OnContinueGameClick -= LoadNextLevel;
            
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(GetNextLevelGameEnterParams());
        }
        private void LoadMetaScene()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadMetaScene();
        }

        
        private GameEnterParams ReceiveGameEnterParams(SceneEnterParams enterParams)
        {
            if (enterParams is not GameEnterParams)
            {
                Debug.LogError("Неправильная передача параметров сцены");
                return null;
            }

            return (GameEnterParams)enterParams;
        }
        private GameEnterParams GetNextLevelGameEnterParams()
        {
            var currentLevel = _levelData.LevelNumber;
            var currentLocation = _levelData.Location;
            var maxLevel = _levels.GetCountMainLevelOnLocation(currentLocation);
            if (currentLevel+1 >= maxLevel)
            {
                currentLevel = 0;
                currentLocation++;
            }
            currentLevel++;
            
            return new GameEnterParams(level: currentLevel, location: currentLocation);
        }
        private void TrySaveEndLevelData()
        {
            _progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
            if (_gameEnterParams.Location == _progress.CurrentLocation &&
                _gameEnterParams.Level == _progress.CurrentLevel)
            {
                var maxLevel = _levels.GetCountMainLevelOnLocation(_progress.CurrentLocation);
                if (_progress.CurrentLevel+1 >= maxLevel)
                {
                    _progress.CurrentLevel = 0;
                    _progress.CurrentLocation++;
                }
                else _progress.CurrentLevel++;
                _saveSystem.SaveData(SavableObjectType.Progress);
            }
        }
        private EndLevelScreenData EditVictoryData()
        {
            var victoryData = _victoryScreenData;
            var totalKills = GameStats.GetKills();
                
            var maxLevelTime = 0f;
            foreach (var levelDataEnemiesWive in _levelData.EnemiesWives.Where(levelDataEnemiesWive => levelDataEnemiesWive.Time > 0)) 
                maxLevelTime = levelDataEnemiesWive.Time;
            if (maxLevelTime > 0)
            {
                var currentTime = maxLevelTime - _timerController.CurrentTime;
                var bestTime = GameStats.SaveBestTime(currentTime);
                    
                victoryData.ThirdLabel = currentTime.ToString("00:00.000s");
                // victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
            }
            victoryData.FirstLabel = victoryData.FirstLabel.Replace("N", totalKills.ToString());
            return victoryData;
        }
    }
}
