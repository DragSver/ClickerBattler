using ClickRPG.CriticalHit;
using ClickRPG.Meta;
using ClickRPG.SceneManagment;
using Game.Configs.Levels;
using UnityEngine;

namespace ClickRPG {
    public class GameController : EntryPoint
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;
        
        [Header("GameScreen")]
        [SerializeField] private ButtonController _clickAttackButtonController;
        [SerializeField] private CriticalHitController _criticalHitController;
        [SerializeField] private LevelsConfig _levels;
        [SerializeField] private Timer _timer;
        [SerializeField] private float _maxLevelTime = 10;
        [SerializeField] private float _damage = 1f;
        [SerializeField] private ButtonData _clickAttackButtonData;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

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
            
            _criticalHitController.Init();
            _enemyController.Init(_timer);
            _clickAttackButtonController.Init(_clickAttackButtonData);
            _clickAttackButtonController.OnClick += () => DamageEnemy(_criticalHitController.GetDamageMultiplierPointerPosition(_damage));

            StartLevel();
        }

        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            
            var levelData = _levels.GetLevel(_gameEnterParams.Location, _gameEnterParams.Level);
            _enemyController.StartLevel(levelData);
            _enemyController.OnLevelPassed += EndLevel;
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
                var currentTime = _maxLevelTime - _timer.CurrentTime;
                var bestTime = GameStats.SaveBestTime(currentTime);
                var totalKills = GameStats.AddKills();
                
                var victoryData = _victoryScreenData;
                victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
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

        private void DamageEnemy(float damage) => _enemyController.DamageCurrentEnemy(damage);

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
    }
}
