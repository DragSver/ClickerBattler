using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Button
{
    public class ButtonController : MonoBehaviour
    {
        public event UnityAction OnClick;

        [SerializeField] private float _cooldownTime;
        [SerializeField] private ButtonView _buttonView;
        private bool _canClick = true;

        public void Init(ButtonData buttonData) 
        {
            _buttonView.Init(
                buttonData.DefaultSprite, 
                buttonData.Text,
                buttonData.ButtonColors,
                buttonData.ImageType);
            
            _buttonView.ClearActionClick();
            _buttonView.SubscribeOnClick(() =>
            {
                if (!_canClick) return;
                
                if (_cooldownTime > 0)
                    _canClick = false;
                OnClick?.Invoke();
                if (!_canClick)
                    StartCoroutine(ClickCooldown());
            });
        }

        public void ClearActionClick() => OnClick = null;

        IEnumerator ClickCooldown()
        {
            yield return new WaitForSeconds(_cooldownTime);
            _canClick = true;
        }
    }
}