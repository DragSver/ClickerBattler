using System.Collections.Generic;
using Configs;
using Datas.Global;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    [SerializeField] private RewardView[] _rewardViews;
    [SerializeField] private ItemsConfig _itemsConfig;

    public void SetRewards(List<CollectedItemsData> collectedItems)
    {
        ClearRewards();
        for (var index = 0; index < collectedItems.Count; index++)
        {
            if (index > _rewardViews.Length - 1) return;
            
            _rewardViews[index].gameObject.SetActive(true);
            _rewardViews[index].SetReward(_itemsConfig.GetItemData(collectedItems[index].Id), collectedItems.Count);
        }
    }

    public void ClearRewards()
    {
        foreach (var rewardView in _rewardViews)
        {
            rewardView.Clear();
            rewardView.gameObject.SetActive(false);
        }
    }
}
