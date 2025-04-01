using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using System.Linq;
using Button;
using Game.Configs.LevelConfigs;
using Game.CriticalHit;
using Game.Enemy;
using Game.Timer;
using SceneManegement;
using SceneManegement.EnterParams;
using UnityEngine;

namespace Game {
    public class GameController : EntryPoint
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;

        [Header("GameScreen")] 
        [SerializeField] private UnityEngine.UI.Button _mapButton;
        [SerializeField] private TimerController _timerController;
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
            _gameEnterParams = ReceiveGameEnterParams(enterParams);
            
            _enemyController.Init(_timerController);
            _levelsViewConfig.Init();

            StartLevel();
        }


        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            
            _mapButton.onClick.AddListener(LoadMetaScene);
            
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
            _timerController.Stop();
            _criticalHitController.StopGenerateCriticalPoint();
            
            _mapButton.onClick.RemoveListener(LoadMetaScene);
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (levelPassed)
            {
                TrySaveEndLevelData();
                
                _endLevelScreenController.OnContinueGameClick += LoadNextLevel;
                _endLevelScreenController.CallEndLevelScreen(EditVictoryData());
            }
            else
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.OnContinueGameClick += LoadRestartLevel;
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
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
                    
                victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
            }
            victoryData.StatisticText = victoryData.StatisticText.Replace("N", totalKills.ToString());
            return victoryData;
        }
    }
}
