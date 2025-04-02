using Datas.Meta;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta.Locations
{
    public class LevelMapViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _locationName;
        [SerializeField] private TextMeshProUGUI _coins;
        [SerializeField] private Image _background;

        [SerializeField] private Button _nextLocationButton;
        [SerializeField] private Button _previousLocationButton;

        [SerializeField] private Button _campButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _shopButton;

        
        public void Init(UnityAction onNextLocation, UnityAction onPreviousLocation, UnityAction onCamp, UnityAction onAchievements, UnityAction onShop)
        {
            InitButton(_nextLocationButton, onNextLocation);
            InitButton(_previousLocationButton, onPreviousLocation);
            InitButton(_campButton, onCamp);
            InitButton(_achievementsButton, onAchievements);
            InitButton(_shopButton, onShop);
        }
        
        public void SetLocation(LevelMapViewData levelMapViewData, int coinsAmount)
        {
            _locationName.text = levelMapViewData.Name;
            _coins.text = coinsAmount.ToString();
            _background.sprite = levelMapViewData.Background;
        }
        public void SetCoinsAmount(int amount)
        {
            _coins.text = amount.ToString();
        }
        
        private void InitButton(Button button, UnityAction action)
        {
            if (action != null)
                button.onClick.AddListener(action);
            button.enabled = action != null;
        }
    }
}