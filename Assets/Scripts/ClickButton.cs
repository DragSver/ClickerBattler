using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG {
    public class ClickButton : MonoBehaviour {
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        
        public void Init(Sprite defaultSprite, ColorBlock buttonColors, Image.Type imageType)
        {
            _button.colors = buttonColors;
            _image.sprite = defaultSprite;
            _image.type = imageType;
        }
        
        public void SubscribeOnClick(UnityAction action) {
            _button.onClick.AddListener(action);
        }
        public void UnsubscribeOnClick(UnityAction action) {
            _button.onClick.RemoveListener(action);
        }
    }
}
