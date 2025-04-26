using System.Collections.Generic;
using Datas.Game;
using Datas.Global;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class EndLevelScreenController : MonoBehaviour
    {
        public event UnityAction OnContinueGameClick;

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

        [SerializeField] private TextMeshProUGUI _continueGameButtonText;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _mapButton;
        
        [SerializeField] private RewardsController _rewardsController;

        [SerializeField] private WalletViewController _walletController;
        
        
        public void CallEndLevelScreen(EndLevelScreenData endLevelScreenData, UnityAction onContinueGameClick, string continueGameButtonText, UnityAction onMapButtonClick, List<CollectedItemsData> collectedItems, int coins, bool win)
        {
            InitButton(_continueGameButton, onContinueGameClick);
            InitButton(_mapButton, onMapButtonClick);
            _continueGameButtonText.text = continueGameButtonText;

            _walletController.UpdateWallet(coins);
            
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
            
            if (collectedItems != null && collectedItems.Count > 0) _rewardsController.SetRewards(collectedItems);
            else _rewardsController.ClearRewards();
            
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
            button.gameObject.SetActive(action != null);
        }
    }
}