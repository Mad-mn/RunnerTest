using Configs.RewardConfigs;

namespace Environment.InsideObjects.Rewards {
  public interface IReward {
    void OnCatch();
    RewardType RewardType {
      get;
      set;
    }
  }
}
