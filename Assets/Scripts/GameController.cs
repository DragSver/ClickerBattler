using UnityEngine;

namespace ClickRPG {
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ClickButtonController _clickButtonController;

        private void Awake()
        {
            _clickButtonController.Init();
        }
    }
}
