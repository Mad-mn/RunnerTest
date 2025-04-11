using System;
using System.Collections.Generic;
using Configs;
using Configs.PoolConfigs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.ObjectPool {
  public class ObjectPoolManager : IObjectPoolManager {

    private readonly IConfigManager _configManager;
    private List<PoolItem> _objectsPool;
    private BasePoolConfig _basePoolConfig;
    private readonly List<PoolItem> _poolItems = new List<PoolItem>();

    private Transform _poolContainer;

    public ObjectPoolManager(IConfigManager configManager) {
      _configManager = configManager;
    }

    public async UniTask Initialize() {
      _basePoolConfig = _configManager.GetConfig<BasePoolConfig>();
      _poolContainer = new GameObject(nameof(_poolContainer)).transform;
      Object.DontDestroyOnLoad(_poolContainer.gameObject);
      await InitializeItems();
    }

    public T GetFromPool<T>() where T : MonoBehaviour, IPoolable {
      PoolItem poolItem = _poolItems.Find(item => item.PollPoolable is T && item.PollPoolable.InPool);
      if (poolItem != null) {
        poolItem.PollPoolable.OnGetFromPool();
        return poolItem.PollPoolable as T;
      }

      Debug.LogError($"Pool item with type {typeof(T)} exist");
      return default;
    }

    public void ReturnToPool<T>(T returnedObject) where T : MonoBehaviour, IPoolable {
      PoolItem poolableItem = _poolItems.Find(item => item.GameObject == returnedObject.gameObject);
      poolableItem.PollPoolable.OnSetToPool();
      poolableItem.GameObject.transform.SetParent(_poolContainer);
    }

    private async UniTask InitializeItems() {
      foreach (PoolPrefabItem poolPrefabItem in _basePoolConfig.Prefabs) {
        for (int i = 0; i < poolPrefabItem.SpawnAmount; i++) {
          GameObject item = Object.Instantiate(poolPrefabItem.Prefab, _poolContainer, true);
          PoolItem poolItem = new PoolItem();
          poolItem.GameObject = item;
          poolItem.PollPoolable = item.GetComponent<IPoolable>();
          poolItem.PollPoolable.Initialize();
          _poolItems.Add(poolItem);
        }

        await UniTask.NextFrame();
      }
    }
  }

  [Serializable]
  public class PoolItem {
    public GameObject GameObject;
    public IPoolable PollPoolable;
  }
}
