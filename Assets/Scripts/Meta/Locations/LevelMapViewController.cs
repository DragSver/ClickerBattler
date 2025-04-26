using Datas.Meta;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta.Locations
{
    public class LevelMapViewController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _locationName;
        [SerializeField] private WalletViewController _walletController;
        [SerializeField] private Image _background;

        public Button NextLocationButton => _nextLocationButton;
        [SerializeField] private Button _nextLocationButton;
        public Button PreviousLocationButton => _previousLocationButton;
        [SerializeField] private Button _previousLocationButton;

        [SerializeField] private Button _campButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _shopButton;

        
        public void Init(UnityAction onNextLocation, UnityAction onPreviousLocation, UnityAction onCamp,
            UnityAction onAchievements, UnityAction onShop, int currentLocation, int countLocations, int wallet)
        {
            InitButton(_nextLocationButton, onNextLocation);
            InitButton(_previousLocationButton, onPreviousLocation);
            InitButton(_campButton, onCamp);
            InitButton(_achievementsButton, onAchievements);
            InitButton(_shopButton, onShop);
            
            SetWallet(wallet);
        }
        
        public void SetLocation(LevelMapViewData levelMapViewData)
        {
            _locationName.text = levelMapViewData.Name;
            _background.sprite = levelMapViewData.Background;
        }
        public void SetWallet(int amount) => _walletController.UpdateWallet(amount);
        
        
        private void InitButton(Button button, UnityAction action)
        {
            if (action != null)
                button.onClick.AddListener(action);
            else
                button.enabled = false;
        }
    }
}