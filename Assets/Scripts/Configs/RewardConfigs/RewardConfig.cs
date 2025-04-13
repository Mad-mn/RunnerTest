using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Configs.RewardConfigs {
  [CreateAssetMenu(menuName = "Configs/Reward/RewardConfig", fileName = "RewardConfig")]
  public class RewardConfig : ScriptableObject {
    [SerializeField]
    private List<RewardItemData> _rewardsData;

    public int GetPointsForItem(RewardType rewardType) {
      RewardItemData data = _rewardsData.FirstOrDefault(reward => reward.RewardType == rewardType);
      return data.PointsForOneItem;
    }
  }

  public enum RewardType {
    Apple,
    Orange,
    Kiwi
  }

  [Serializable]
  public struct RewardItemData {
    public RewardType RewardType;
    public int PointsForOneItem;
  }
}
