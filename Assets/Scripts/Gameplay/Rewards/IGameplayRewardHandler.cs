using Configs.RewardConfigs;

namespace Gameplay.Rewards {
  public interface IGameplayRewardHandler {
    void IncreaseRewards(RewardType rewardType);
    void Save();
  }
}
