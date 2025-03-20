using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG.Meta.Locations
{
    public class PinView : MonoBehaviour
    {
        [SerializeField] private Button _levelButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _completeImage;
        [SerializeField] private Image _currentImage;
        

        public void Init(int levelNumber, PinType pinType, UnityAction clickCallback)
        {
            _text.text = levelNumber.ToString();

            switch (pinType)
            {
                case PinType.Complete:
                    _completeImage.gameObject.SetActive(true);
                    break;
                case PinType.Current:
                    _currentImage.gameObject.SetActive(true);
                    break;
                case  PinType.Closed:
                    _levelButton.enabled = false;
                    break;
            }
            
            _levelButton.onClick.AddListener(() => clickCallback?.Invoke());
        }
    }
}