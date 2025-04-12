using System.Collections.Generic;
using Configs.RewardConfigs;
using UnityEngine;

namespace UI.Windows.Gameplay {
  public class RewardVisualController : MonoBehaviour {
    [SerializeField]
    private List<RewardCountItem> _rewardCountItems;

    public void SetToDefault() {
      foreach (RewardCountItem item in _rewardCountItems) {
        item.SetToDefault();
      }
    }

    public void OnPlayerCatchReward(RewardType rewardType) {
      RewardCountItem item = _rewardCountItems.Find(item => item.RewardType == rewardType);
      item.IncreaseCount();
    }
  }
}
