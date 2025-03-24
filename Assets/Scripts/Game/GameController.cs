using ClickRPG.CriticalHit;
using ClickRPG.Meta;
using ClickRPG.SceneManagment;
using UnityEngine;

namespace ClickRPG {
    public class GameController : EntryPoint
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;
        
        [Header("GameScreen")]
        [SerializeField] private ButtonController _clickAttackButtonController;
        [SerializeField] private CriticalHitController _criticalHitController;
        [SerializeField] private Timer _timer;
        [SerializeField] private float _maxLevelTime = 10;
        [SerializeField] private float _damage = 1f;
        [SerializeField] private ButtonData _clickAttackButtonData;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private const string SCENE_LOADER_TAG = "SceneLoader";

        
        public override void Run(SceneEnterParams enterParams)
        {
            var gameParams = enterParams as GameEnterParams;
            StartLevel();
        }

        private void StartLevel()
        {
            _criticalHitController.Init();
            // _endLevelScreenController.OnContinueGameClick -= StartLevel;
            
            _endLevelScreenController.HideEndLevelScreen();
            _enemyController.Init();
            _enemyController.OnDead += EndLevel;
            
            _clickAttackButtonController.Init(_clickAttackButtonData);
            _clickAttackButtonController.OnClick += () => DamageEnemy(_criticalHitController.GetDamageMultiplierPointerPosition(_damage));
            
            _timer.Init(_maxLevelTime);
            _criticalHitController.StartGenerateCriticalPoint();
            _timer.Play();
            _timer.OnTimerEnd += EndLevel;
        }

        private void EndLevel()
        {
            _timer.Stop();
            _criticalHitController.StopGenerateCriticalPoint();
            // _enemyController.OnDead -= EndLevel;
            // _enemyController.ClearEnemy();
            // _timer.OnTimerEnd -= EndLevel;
            // _clickAttackButtonController.ClearActionClick();
            
            _endLevelScreenController.OnContinueGameClick += RestartLevel;
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (_timer.CurrentTime == 0)
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
            }
            else
            {
                var currentTime = _maxLevelTime - _timer.CurrentTime;
                var bestTime = GameStats.SaveBestTimeEnemy(currentTime, _enemyController.CurrentEnemyData.EnemyId);
                var totalKills = GameStats.AddKills();
                
                var victoryData = _victoryScreenData;
                victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
                victoryData.StatisticText = victoryData.StatisticText.Replace("N", totalKills.ToString());

                _endLevelScreenController.CallEndLevelScreen(victoryData);
            }
            
            _enemyController.ClearEnemy();
        }

        private void DamageEnemy(float damage) => _enemyController.DamageCurrentEnemy(damage);

        private void RestartLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene();
        }
        
        private void LoadMetaScene()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadMetaScene();
        }
    }
}
