using Configs.RewardConfigs;
using Core.SaveLoadDataSystem;
using Core.SaveLoadDataSystem.SavedData;

namespace Gameplay.Rewards {
  public class GameplayRewardHandler : IGameplayRewardHandler {
    private readonly IDataHandler _dataHandler;
    private readonly RewardConfig _rewardConfig;
    private int _totalPoints;

    public GameplayRewardHandler(IDataHandler dataHandler, RewardConfig rewardConfig) {
      _dataHandler = dataHandler;
      _rewardConfig = rewardConfig;
    }

    public void IncreaseRewards(RewardType rewardType) {
      _totalPoints += _rewardConfig.GetPointsForItem(rewardType);
    }

    public void Save() {
      _dataHandler.GetData<PlayerData>().SetupNewResult(_totalPoints);
      _dataHandler.Save();
    }
  }
}
