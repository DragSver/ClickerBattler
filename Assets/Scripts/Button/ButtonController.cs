using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG
{
    public class ButtonController : MonoBehaviour
    {
        public event UnityAction OnClick;
        
        [SerializeField] private ButtonView _buttonView;

        public void Init(ButtonData buttonData) 
        {
            _buttonView.Init(
                buttonData.DefaultSprite, 
                buttonData.Text,
                buttonData.ButtonColors,
                buttonData.ImageType);
            
            _buttonView.ClearActionClick();
            _buttonView.SubscribeOnClick(() => OnClick?.Invoke());
        }

        public void ClearActionClick() => OnClick = null;
    }
}