using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta.Locations
{
    public class PinView : MonoBehaviour
    {
        [SerializeField] private Button _levelButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _completeImage;
        [SerializeField] private Image _currentImage;
        

        public void Init(int levelNumber, ProgressState progressState, UnityAction clickCallback)
        {
            _text.text = levelNumber.ToString();

            switch (progressState)
            {
                case ProgressState.Complete:
                    _completeImage.gameObject.SetActive(true);
                    break;
                case ProgressState.Current:
                    _currentImage.gameObject.SetActive(true);
                    break;
                case  ProgressState.Closed:
                    _levelButton.enabled = false;
                    break;
            }
            
            _levelButton.onClick.AddListener(() => clickCallback?.Invoke());
        }
    }
}