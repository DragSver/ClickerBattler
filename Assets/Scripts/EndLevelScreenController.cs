using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG
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

        [SerializeField] private TextMeshProUGUI _statisticsText;
        [SerializeField] private TextMeshProUGUI _killTimeAndAdviceText;

        [SerializeField] private GameObject _killTimeAndAdviceTextHolder;
        [SerializeField] private Image _timeImage;

        [SerializeField] private ButtonController _continueGameButtonController;
        [SerializeField] private ButtonController _mapButtonController;

        
        public void CallEndLevelScreen(EndLevelScreenData endLevelScreenData)
        {
            _continueGameButtonController.Init(endLevelScreenData.ContinueGameButtonData);
            _mapButtonController.Init(endLevelScreenData.MapButtonData);

            _continueGameButtonController.OnClick += () => OnContinueGameClick?.Invoke();
            _mapButtonController.OnClick += () => OnMapButtonClick?.Invoke();

            _background.sprite = endLevelScreenData.Background;
            _flagImage.sprite = endLevelScreenData.Flag;

            _colorMainTextHolder.color = endLevelScreenData.ColorMainTextHolder;

            InitTMP(_mainText, endLevelScreenData.MainText, endLevelScreenData.ColorMainText,
                endLevelScreenData.MaterialMainText);
            InitTMP(_statisticsText, endLevelScreenData.StatisticText, endLevelScreenData.ColorAdviceText,
                endLevelScreenData.MaterialAdviceText);
            if (endLevelScreenData.KillTimeText == string.Empty)
            {
                InitTMP(_killTimeAndAdviceText, endLevelScreenData.AdviceText, endLevelScreenData.ColorAdviceText,
                    endLevelScreenData.MaterialAdviceText);
                _timeImage.gameObject.SetActive(false);
            }
            else 
            {
                InitTMP(_killTimeAndAdviceText, endLevelScreenData.KillTimeText, endLevelScreenData.ColorAdviceText,
                    endLevelScreenData.MaterialAdviceText);
                _timeImage.gameObject.SetActive(true);
            }
            
            _endLevelScreen.SetActive(true);
        }

        public void HideEndLevelScreen() 
        {
            _continueGameButtonController.OnClick -= () => OnContinueGameClick?.Invoke();
            _mapButtonController.OnClick -= () => OnMapButtonClick?.Invoke();
            _endLevelScreen.SetActive(false);
        }

        private void InitTMP(TextMeshProUGUI initTMP, string text, Color color, Material material)
        {
            initTMP.text = text;
            initTMP.color = color;
            initTMP.fontMaterial = material;
        } 
    }

    [Serializable]
    public struct EndLevelScreenData
    {
        public Sprite Background;

        public Sprite Flag;

        public Color ColorMainTextHolder;

        public string MainText;
        public Color ColorMainText;
        public Material MaterialMainText;

        public string StatisticText;
        public string AdviceText;
        public string KillTimeText;
        public Color ColorAdviceText;
        public Material MaterialAdviceText;

        public ButtonData ContinueGameButtonData;
        public ButtonData MapButtonData;
    } 
}