using System;
using Configs.RewardConfigs;
using Core.Managers.Input;
using Core.ObjectPool;
using Environment.InsideObjects.Obstacles;
using Environment.InsideObjects.Rewards;
using UnityEngine;

namespace Player {
  public class PlayerController : MonoBehaviour, IPoolable {
    public event Action<RewardType> OnCatchReward;
    public event Action OnCollideWithObstacle;
    [SerializeField]
    private PlayerMovementController _movementController;

    public void Initialize(PlayerMovementData movementData, IInputHandler inputHandler) {
      _movementController.SetupData(movementData, inputHandler);
    }

    private void OnTriggerEnter(Collider other) {
      if (CheckRewardTrigger(other)) {
        return;
      }

      CheckObstacleTrigger(other);
    }

    public void OnStartGame() {
      _movementController.StartRun();
    }

    public void Initialize() {
      ReturnToPool();
    }

    public void OnGetFromPool() {
      gameObject.SetActive(true);
      InPool = false;
    }

    public void ReturnToPool() {
      gameObject.SetActive(false);
      InPool = true;
    }

    private bool CheckRewardTrigger(Collider col) {
      if (col.TryGetComponent(out IReward reward)) {
        OnCatchReward?.Invoke(reward.RewardType);
        reward.OnCatch();
        return true;
      }

      return false;
    }

    private void CheckObstacleTrigger(Collider col) {
      if (col.TryGetComponent(out BaseObstacle obstacle)) {
        _movementController.Stop();
        OnCollideWithObstacle?.Invoke();
      }
    }

    public bool InPool {
      get;
      set;
    }
  }
}
