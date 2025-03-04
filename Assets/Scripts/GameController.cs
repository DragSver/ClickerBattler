using UnityEngine;

namespace ClickRPG {
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ClickButtonController _clickButtonController;
        [SerializeField] private EnemyController _enemyController;

        private void Awake()
        {
            _clickButtonController.Init();
            _enemyController.Init();

            _clickButtonController.OnClick += () => _enemyController.DamageCurrentEnemy(1f);
        }
    }
}
