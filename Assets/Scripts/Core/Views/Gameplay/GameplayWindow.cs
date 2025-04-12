using Configs.RewardConfigs;
using UI.Windows;
using UI.Windows.Gameplay;
using UnityEngine;

namespace Core.Views.Gameplay {
  public class GameplayWindow : BaseUIWindow {
    [SerializeField]
    private RewardVisualController _rewardVisualController;

    private void OnEnable() {
      _rewardVisualController.SetToDefault();
    }

    public void OnPlayerCatchReward(RewardType rewardType) {
      _rewardVisualController.OnPlayerCatchReward(rewardType);
    }
  }
}
