using System.Globalization;
using TMPro;
using UnityEngine;

namespace Global
{
    public class WalletViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _walletText;

        public void UpdateWallet(int coins)
        {
            _walletText.text = coins.ToString("N0", new CultureInfo("ru-RU"));
        }
    }
}