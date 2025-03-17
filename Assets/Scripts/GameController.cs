using UnityEngine;

namespace ClickRPG {
    public class GameController : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;

        
        [Header("GameScreen")]
        [SerializeField] private ButtonController _clickAttackButtonController;
        [SerializeField] private ButtonData _clickAttackButtonData;
        [SerializeField] private Timer _timer;
        [SerializeField] private float _maxLevelTime = 10;
        [SerializeField] private float _damage = 1f;
        private GameStats _gameStats;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private void Awake()
        {
            _gameStats = new GameStats();
            StartLevel();
        }

        private void StartLevel()
        {
            _endLevelScreenController.OnContinueGameClick -= StartLevel;
            
            _endLevelScreenController.HideEndLevelScreen();
            _enemyController.Init();
            _enemyController.OnDead += EndLevel;
            
            _clickAttackButtonController.Init(_clickAttackButtonData);
            _clickAttackButtonController.OnClick += DamageEnemy;
            
            _timer.Init(_maxLevelTime);
            _timer.Play();
            _timer.OnTimerEnd += EndLevel;
        }

        private void EndLevel()
        {
            _timer.Stop();
            
            _enemyController.OnDead -= EndLevel;
            _enemyController.ClearEnemy();
            _timer.OnTimerEnd -= EndLevel;
            _clickAttackButtonController.ClearActionClick();
            
            _endLevelScreenController.OnContinueGameClick += StartLevel;

            if (_timer.CurrentTime == 0)
            {
                var totalDeaths = _gameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
            }
            else
            {
                var currentTime = _maxLevelTime - _timer.CurrentTime;
                var bestTime = _gameStats.SaveBestTimeEnemy(currentTime, _enemyController.CurrentEnemyData.EnemyId);
                var totalKills = _gameStats.AddKills();
                
                var victoryData = _victoryScreenData;
                victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
                victoryData.StatisticText = victoryData.StatisticText.Replace("N", totalKills.ToString());

                _endLevelScreenController.CallEndLevelScreen(victoryData);
            }
            
            _enemyController.ClearEnemy();
        }

        private void DamageEnemy() => _enemyController.DamageCurrentEnemy(_damage);
    }
}
