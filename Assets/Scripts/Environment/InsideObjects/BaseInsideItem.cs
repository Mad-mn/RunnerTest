using Core.ObjectPool;
using UnityEngine;
using Zenject;
using IPoolable = Core.ObjectPool.IPoolable;

namespace Environment.InsideObjects {
  public class BaseInsideItem : MonoBehaviour, IPoolable {

    protected IObjectPoolManager _objectPoolManager;

    public void Initialize() {
      _objectPoolManager = ProjectContext.Instance.Container.Resolve<IObjectPoolManager>();
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
