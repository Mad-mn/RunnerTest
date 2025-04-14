using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.PoolConfigs;
using Core.Loaders.AssetLoaders;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.ObjectPool {
  public class ObjectPoolManager : IObjectPoolManager, IAssetLoader {

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

    public int LoadAssetOrder {
      get { return 3; }
    }

    public T GetFromPool<T>() where T : MonoBehaviour, IPoolable {
      PoolItem poolItem = _poolItems.Find(item => item.PollPoolable is T && item.PollPoolable.InPool);
      if (poolItem != null) {
        poolItem.PollPoolable.OnGetFromPool();
        return poolItem.PollPoolable as T;
      }

      Debug.Log($"There is no free item {typeof(T)}. Try create new.");

      poolItem =  _poolItems.FirstOrDefault(item => item.PollPoolable is T);

      if (poolItem != default) {
        PoolItem newItem = CreateItem(poolItem.GameObject);
        _poolItems.Add(newItem);
        return newItem.PollPoolable as T;
      }

      Debug.LogError($"Pool item with type {typeof(T)} exist.");
      return default;
    }

    public T GetFromPool<T>(Type objType) where T : MonoBehaviour, IPoolable {
      PoolItem poolItem = _poolItems.Find(item => item.Type == objType && item.PollPoolable.InPool);
      if (poolItem != null) {
        poolItem.PollPoolable.OnGetFromPool();
        return poolItem.PollPoolable as T;
      }

      Debug.Log($"There is no free item {typeof(T)}. Try create new.");
      poolItem =  _poolItems.FirstOrDefault(item => item.Type == objType);

      if (poolItem != default) {
        PoolItem newItem = CreateItem(poolItem.GameObject);
        _poolItems.Add(newItem);
        return newItem.PollPoolable as T;
      }

      Debug.LogError($"Pool item with type {typeof(T)} exist.");
      return default;
    }

    public void ReturnToPool<T>(T returnedObject) where T : MonoBehaviour, IPoolable {
      PoolItem poolableItem = _poolItems.Find(item => item.GameObject == returnedObject.gameObject);
      poolableItem.PollPoolable.ReturnToPool();
      poolableItem.GameObject.transform.SetParent(_poolContainer);
    }

    private async UniTask InitializeItems() {
      foreach (PoolPrefabItem poolPrefabItem in _basePoolConfig.Prefabs) {
        GameObject prefab = await poolPrefabItem.AssetReference.LoadAssetAsync<GameObject>();
        for (int i = 0; i < poolPrefabItem.SpawnAmount; i++) {
          _poolItems.Add(CreateItem(prefab));
        }

        await UniTask.NextFrame();
      }
    }

    private PoolItem CreateItem(GameObject prefab) {
      GameObject item = Object.Instantiate(prefab, _poolContainer, true);
      PoolItem poolItem = new PoolItem();
      poolItem.GameObject = item;
      poolItem.PollPoolable = item.GetComponent<IPoolable>();
      poolItem.Type = poolItem.PollPoolable.GetType();
      poolItem.PollPoolable.Initialize();
      return poolItem;
    }
  }

  [Serializable]
  public class PoolItem {
    public GameObject GameObject;
    public IPoolable PollPoolable;
    public Type Type;
  }
}
