using UnityEngine;

namespace ClickRPG
{
    public class ClickButtonController : MonoBehaviour
    {
        [SerializeField] private ClickButton _clickButton;
        [SerializeField] private ClickButtonConfig _buttonConfig;

        public void Init() {
            _clickButton.Init(_buttonConfig.DefaultSprite, _buttonConfig.ButtonColors, _buttonConfig.ImageType);
            _clickButton.SubscribeOnClick(ShowClick);
            _buttonConfig.OnReinitialize += () =>
            {
                _clickButton.Init(_buttonConfig.DefaultSprite, _buttonConfig.ButtonColors,
                    _buttonConfig.ImageType);
            };
        }

        private void ShowClick() {
            Debug.Log("Click");
        }
    }
}