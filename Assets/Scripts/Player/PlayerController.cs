using System;
using Configs.RewardConfigs;
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

    public void Initialize(PlayerMovementData movementData) {
      _movementController.SetupData(movementData);
    }

    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent(out IReward reward)) {
        OnCatchReward?.Invoke(reward.RewardType);
        reward.OnCatch();
        return;
      }

      if (other.TryGetComponent(out BaseObstacle obstacle)) {
        _movementController.Stop();
        OnCollideWithObstacle?.Invoke();
      }
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

    public bool InPool {
      get;
      set;
    }
  }
}
