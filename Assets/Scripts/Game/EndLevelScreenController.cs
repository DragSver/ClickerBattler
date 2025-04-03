using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class EndLevelScreenController : MonoBehaviour
    {
        public event UnityAction OnContinueGameClick;
        public event UnityAction OnMapButtonClick;

        [SerializeField] private GameObject _endLevelScreen;

        [SerializeField] private Image _background;
        [SerializeField] private Image _flagImage;

        [SerializeField] private Image _colorMainTextHolder;

        [SerializeField] private TextMeshProUGUI _mainText;

        [SerializeField] private TextMeshProUGUI _firstTitle;
        [SerializeField] private TextMeshProUGUI _secondTitle;
        [SerializeField] private TextMeshProUGUI _thirdTitle;

        [SerializeField] private Image _timeImage;
        [SerializeField] private Image _bestTimeImage;

        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _mapButton;
        
        
        public void CallEndLevelScreen(Datas.Game.EndLevelScreenData endLevelScreenData, UnityAction onContinueGameClick, UnityAction onMapButtonClick, bool win)
        {
            InitButton(_continueGameButton, onContinueGameClick);
            InitButton(_mapButton, onMapButtonClick);
            
            _background.sprite = endLevelScreenData.Background;
            _flagImage.sprite = endLevelScreenData.Flag;

            _colorMainTextHolder.color = endLevelScreenData.ColorMainTextHolder;

            InitTMP(_mainText, endLevelScreenData.MainText, endLevelScreenData.ColorMainText);
            InitTMP(_firstTitle, endLevelScreenData.FirstLabel, endLevelScreenData.ColorAdviceText);
            InitTMP(_secondTitle, endLevelScreenData.SecondLabel, endLevelScreenData.ColorAdviceText);
            InitTMP(_thirdTitle, endLevelScreenData.ThirdLabel, endLevelScreenData.ColorAdviceText);

            if (win)
            {
                _timeImage.gameObject.SetActive(true);
                _bestTimeImage.gameObject.SetActive(true);
            }
            else
            {
                _timeImage.gameObject.SetActive(false);
                _bestTimeImage.gameObject.SetActive(false);
            }
            
            _endLevelScreen.SetActive(true);
        }

        public void HideEndLevelScreen() => 
            _endLevelScreen.SetActive(false);

        private void InitTMP(TextMeshProUGUI initTMP, string text, Color color)
        {
            initTMP.text = text;
            initTMP.color = color;
        }
        private void InitButton(Button button, UnityAction action)
        {
            if (action != null)
                button.onClick.AddListener(action);
            button.enabled = action != null;
        }
    }
}