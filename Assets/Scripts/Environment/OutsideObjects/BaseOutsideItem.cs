using Core.ObjectPool;
using UnityEngine;

namespace Environment.OutsideObjects {
  public class BaseOutsideItem : MonoBehaviour, IPoolable {
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
