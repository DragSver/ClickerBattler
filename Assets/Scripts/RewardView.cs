using System.Globalization;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardCountText;
    [SerializeField] private Image _rewardImage;

    public void SetReward(ItemData itemData, int count)
    {
        _rewardCountText.text = count.ToString("N0", new CultureInfo("ru-RU"));;
        _rewardImage.sprite = itemData.Sprite;
    }

    public void Clear()
    {
        _rewardCountText.text = "";
        _rewardImage.sprite = null;
    }
}
