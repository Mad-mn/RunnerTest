using System;
using System.Collections;
using Configs.RewardConfigs;
using Cysharp.Threading.Tasks;
using TMPro;
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
    [SerializeField]
    private TMP_Text _timerTxt;

    private Coroutine _timerRoutine;

    private void OnEnable() {
      _rewardVisualController.SetToDefault();
      AddListeners();
    }

    private void OnDisable() {
      RemoveListeners();
      if (_timerRoutine == null) {
        return;
      }

      StopCoroutine(_timerRoutine);
      _timerRoutine = null;
    }

    public void OnPlayerCatchReward(RewardType rewardType) {
      _rewardVisualController.OnPlayerCatchReward(rewardType);
    }

    public void OnPlayerCollideWithObstacle() {
      _gameoverPanel.GameOver();
    }

    public void StartTimer(int seconds) {
      _timerRoutine = StartCoroutine(TimerRoutine(seconds));
    }

    private IEnumerator TimerRoutine(int seconds) {
      _timerTxt.gameObject.SetActive(true);
      WaitForSeconds second = new WaitForSeconds(1);
      while (seconds > 0) {
        _timerTxt.text = seconds.ToString();
        yield return second;
        seconds--;
      }

      _timerTxt.gameObject.SetActive(false);
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

    public override UniTask Initialize() {
      return UniTask.CompletedTask;
    }
  }
}
