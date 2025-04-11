using System;
using Core.ObjectPool;
using Tools.Constants;
using UnityEngine;

namespace Environment.Road {
  public class RoadItem : MonoBehaviour, IPoolable {

    public event Action<RoadItem> OnPlayerEnter;
    [SerializeField]
    private Transform _enterPosition;
    [SerializeField]
    private Transform _exitPosition;

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag(TagLayerNames.PlayerTag)) {
        OnPlayerEnter?.Invoke(this);
      }
    }

    public Vector3 EnterPosition { get { return _enterPosition.position; } }
    public Vector3 ExitPosition { get { return _exitPosition.position; } }

    public void Initialize() {
      OnSetToPool();
    }

    public void OnGetFromPool() {
      gameObject.SetActive(true);
      InPool = false;
    }

    public void OnSetToPool() {
      gameObject.SetActive(false);
      InPool = true;
    }

    public bool InPool {
      get;
      set;
    }
  }
}
