using TMPro;
using UnityEngine;

namespace Global
{
    public class WalletViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _walletText;

        public void UpdateWallet(int coins)
        {
            _walletText.text = coins > 1000000? $"{coins/1000000}кк": coins > 1000? $"{coins/1000}к" : coins.ToString();
        }
    }
}