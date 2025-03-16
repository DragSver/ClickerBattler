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

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private void Awake()
        {
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
            _timer.OnTimerEnd -= EndLevel;
            _clickAttackButtonController.ClearActionClick();
            
            _endLevelScreenController.OnContinueGameClick += StartLevel;

            var victoryData = _victoryScreenData;
            victoryData.KillTimeText = (_maxLevelTime - _timer.CurrentTime).ToString("00:00.000s");
            
            _endLevelScreenController.CallEndLevelScreen(_timer.CurrentTime == 0
                ? _loseScreenData
                : victoryData);
        }

        private void DamageEnemy() => _enemyController.DamageCurrentEnemy(_damage);
    }
}
