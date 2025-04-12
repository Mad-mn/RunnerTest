using Core.ObjectPool;
using UnityEngine;

namespace Environment.InsideObjects {
  public class BaseInsideItem : MonoBehaviour, IPoolable {

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
