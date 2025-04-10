using Core.ObjectPool;
using UnityEngine;

namespace Environment.Road {
  public class RoadItem : MonoBehaviour, IPoolable {
    [SerializeField]
    private Transform _enterPosition;
    [SerializeField]
    private Transform _exitPosition;

    public Transform EnterPosition { get { return _enterPosition; } }
    public Transform ExitPosition { get { return _exitPosition; } }

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
