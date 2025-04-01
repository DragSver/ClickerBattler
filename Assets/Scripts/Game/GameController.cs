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
        private const string SCENE_LOADER_TAG = "SceneLoader";

        
        public override void Run(SceneEnterParams enterParams)
        {
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
            
            _endLevelScreenController.OnContinueGameClick += RestartLevel;
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (levelPassed)
            {
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

                _endLevelScreenController.CallEndLevelScreen(victoryData);
            }
            else
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
            }
        }

        private void DamageEnemy(Enemy enemy, Elements element, float damage) =>
            _enemyController.DamageCurrentEnemy(enemy, element, damage);

        private void RestartLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(_gameEnterParams);
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
