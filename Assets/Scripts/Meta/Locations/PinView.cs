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
        [SerializeField] private Image _image;
        [SerializeField] private Color _completeColor;
        [SerializeField] private Color _closeColor;
        

        public void Init(int levelNumber, ProgressState progressState, UnityAction clickCallback)
        {
            _text.text = (levelNumber+1).ToString();

            switch (progressState)
            {
                case ProgressState.Closed:
                    _image.color = _closeColor;
                    break;
                case ProgressState.Complete:
                    _image.color = _completeColor;
                    break;
            }

            _levelButton.enabled = progressState != ProgressState.Closed;
            _levelButton.onClick.AddListener(() => clickCallback?.Invoke());
        }
    }
}