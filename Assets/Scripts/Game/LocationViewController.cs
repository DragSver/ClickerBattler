using Datas.Game;
using Game.AttackButtons;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class LocationViewController : MonoBehaviour
    {
        public Canvas Canvas => _canvas;
        [SerializeField] private Canvas _canvas;
        
        [Header("InfoArea")]
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private WalletViewController _walletController;
        
        [Header("GameArea")]
        [SerializeField] private Image _background;
        [SerializeField] private Transform _attackButtonControllerHolder;
        
        [Header("TimerArea")]
        [SerializeField] private Image _timerFill;
        [SerializeField] private Image _timerBackGround;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        [Header("ButtonsArea")]
        [SerializeField] private Button _mapButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _pauseButton;

        public AttackButtonController AttackButtonController => _attackButtonController;
        private AttackButtonController _attackButtonController;

        public void SetLocationView(LocationViewData locationViewData, 
            int level, int maxLevel, int countCoins,
            UnityAction onMapButtonClicked,
            UnityAction onShopButtonClicked,
            UnityAction onPauseButtonClicked)
        {
            _name.text = $"{locationViewData.Name} ур. {level}/{maxLevel}";
            _walletController.UpdateWallet(countCoins);
            
            _background.sprite = locationViewData.Background;
            _attackButtonController = Instantiate(locationViewData.AttackButtonControllerPrefab, _attackButtonControllerHolder);
            
            _timerFill.color = locationViewData.TimerColor;

            if (onMapButtonClicked != null)
                _mapButton.onClick.AddListener(onMapButtonClicked);
            else _mapButton.enabled = false;
            if (onShopButtonClicked != null)
                _shopButton.onClick.AddListener(onShopButtonClicked);
            else _shopButton.enabled = false;
            if (onPauseButtonClicked != null)
                _pauseButton.onClick.AddListener(onPauseButtonClicked);
            else _pauseButton.enabled = false;
        }

        public void ClearLocationView()
        {
            _background.sprite = null;
            _name.text = "";
            if (_attackButtonController != null)
                Destroy(_attackButtonController.gameObject);
            _timerFill.color = Color.white;
            
            _mapButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _pauseButton.onClick.RemoveAllListeners();
        }
    }
}