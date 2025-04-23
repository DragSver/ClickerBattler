using System.Collections;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using UnityEngine;
using UnityEngine.Events;
using YG;

namespace Meta.RewardedAd
{
    public class RewardedAdController : MonoBehaviour
    {
        [SerializeField] private ConfirmWindow _confirmWindow;
        
        private UnityAction<UnityAction> _showRewardedAd;
        private UnityAction _hideRewardedAd;
        private UnityAction _updateWallet;
        private SaveSystem _saveSystem;
        private Wallet _wallet;
        private int _reward;
        
        
        public void Init(UnityAction<UnityAction> showRewardButton, UnityAction hideRewardButton, SaveSystem saveSystem, int reward, UnityAction updateWallet)
        {
            _saveSystem = saveSystem;
            _wallet = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            _showRewardedAd = showRewardButton;
            _hideRewardedAd = hideRewardButton;
            _updateWallet = updateWallet;
            _reward = reward;
            
            showRewardButton?.Invoke(OnRewardShow);
        }

        public void SetReward(int reward)
        {
            _reward = reward;
        }
        
        private void OnRewardShow()
        {
            _confirmWindow.CallWindowInfo(ShowAdvertisement, null, $"Посмотреть рекламу и получить {_reward} монет?");
        }

        private void ShowAdvertisement()
        {
            YG2.RewardedAdvShow("shopCoinReward", GetReward);
            _hideRewardedAd?.Invoke();
            StartCoroutine(WaitAdShow());
        }

        private IEnumerator WaitAdShow()
        {
            yield return new WaitForSeconds(120f);
            _showRewardedAd?.Invoke(OnRewardShow);
        }

        private void GetReward()
        {
            _wallet.Coins += _reward;
            _saveSystem.SaveData(SavableObjectType.Wallet);
            _updateWallet?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}