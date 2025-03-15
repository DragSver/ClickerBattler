using UnityEngine;

namespace ClickRPG {
    public class GameController : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;

        [Header("ClickAttack")]
        [SerializeField] private ButtonController _clickAttackButtonController;
        [SerializeField] private ButtonData _clickAttackButtonData;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private void Awake()
        {
            _clickAttackButtonController.Init(_clickAttackButtonData);
            _enemyController.Init();
            _enemyController.OnDead += () => _endLevelScreenController.CallEndLevelScreen(_victoryScreenData);

            _endLevelScreenController.OnContinueGameClick += () =>
            {
                _endLevelScreenController.HideEndLevelScreen();
                _enemyController.Init();
            };

            _clickAttackButtonController.OnClick += () => _enemyController.DamageCurrentEnemy(1f);
            
        }
    }
}
