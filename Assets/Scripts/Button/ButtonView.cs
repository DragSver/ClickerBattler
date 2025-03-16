using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG {
    public class ButtonView : MonoBehaviour {
        
        [field: SerializeField] private Button Button { get; set; }
        [field: SerializeField] private Image Image { get; set; }
        [field: SerializeField] private TextMeshProUGUI Text { get; set; }

        
        public void Init(Sprite defaultSprite, string buttonText, ColorBlock buttonColors, Image.Type imageType)
        {
            Button.colors = buttonColors;
            Image.sprite = defaultSprite;
            Image.type = imageType;
            Text.text = buttonText;
        }
        
        public void SubscribeOnClick(UnityAction action) {
            Button.onClick.AddListener(action);
        }
        public void UnsubscribeOnClick(UnityAction action) {
            Button.onClick.RemoveListener(action);
        }

        public void ClearActionClick() => Button.onClick.RemoveAllListeners();
    }
}
