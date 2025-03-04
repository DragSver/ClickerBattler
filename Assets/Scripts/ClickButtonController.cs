using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG
{
    public class ClickButtonController : MonoBehaviour
    {
        public event UnityAction OnClick;
        
        [SerializeField] private ClickButton _clickButton;
        [SerializeField] private ClickButtonConfig _buttonConfig;

        public void Init() {
            _clickButton.Init(_buttonConfig.DefaultSprite, _buttonConfig.ButtonColors, _buttonConfig.ImageType);
            _clickButton.SubscribeOnClick(() => OnClick?.Invoke());
            
            _buttonConfig.OnReinitialize += () =>
            {
                _clickButton.Init(_buttonConfig.DefaultSprite, _buttonConfig.ButtonColors,
                    _buttonConfig.ImageType);
            };
        }

    }
}