using Configs.RewardConfigs;
using UnityEngine;

namespace Environment.InsideObjects.Rewards {
  public class BaseRewardItem : BaseInsideItem, IReward {
    [SerializeField]
    private RewardType _rewardType;

    public void OnCatch() {
      ReturnToPool();
    }

    public RewardType RewardType {
      get { return _rewardType; }
      set {}
    }
  }
}
