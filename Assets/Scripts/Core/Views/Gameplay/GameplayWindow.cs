using System;
using Configs.RewardConfigs;
using UI.Windows;
using UI.Windows.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Gameplay {
  public class GameplayWindow : BaseUIWindow {
    public event Action OnExit;
    [SerializeField]
    private RewardVisualController _rewardVisualController;
    [SerializeField]
    private GameoverPanelController _gameoverPanel;
    [SerializeField]
    private Button _exitButton;

    private void OnEnable() {
      _rewardVisualController.SetToDefault();
      AddListeners();
    }

    private void OnDisable() {
      RemoveListeners();
    }

    public void OnPlayerCatchReward(RewardType rewardType) {
      _rewardVisualController.OnPlayerCatchReward(rewardType);
    }

    public void OnPlayerCollideWithObstacle() {
      _gameoverPanel.GameOver();
    }

    private void AddListeners() {
      _exitButton.onClick.AddListener(OnExitButton);
    }

    private void RemoveListeners() {
      _exitButton.onClick.RemoveListener(OnExitButton);
    }

    private void OnExitButton() {
      OnExit?.Invoke();
    }

  }
}
