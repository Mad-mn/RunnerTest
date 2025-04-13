using System;
using System.Collections.Generic;
using Core.Managers.RoadEnvironment;
using Core.ObjectPool;
using Environment.InsideObjects;
using Environment.OutsideObjects;
using Tools.Constants;
using UnityEngine;
using Zenject;
using IPoolable = Core.ObjectPool.IPoolable;

namespace Environment.Road {
  public class RoadItem : MonoBehaviour, IPoolable {
    public event Action<RoadItem> OnPlayerEnter;
    [SerializeField]
    private Transform _enterPosition;
    [SerializeField]
    private Transform _exitPosition;
    [SerializeField]
    private List<Transform> _outsidePositions;
    [SerializeField]
    private List<Transform> _insidePositions;

    private IRoadEnvironmentSpawnManager _roadEnvironmentSpawnManager;
    private IObjectPoolManager _objectPoolManager;

    private List<BaseOutsideItem> _outsideItems;
    private List<BaseInsideItem> _obstacles;
    private List<BaseInsideItem> _rewards;

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag(TagLayerNames.PlayerTag)) {
        OnPlayerEnter?.Invoke(this);
      }
    }

    public void SetupEnvironment(bool setupInside) {
      _outsideItems = _roadEnvironmentSpawnManager.SpawnOutside(_outsidePositions, transform);
      if (!setupInside) {
        return;
      }

      _roadEnvironmentSpawnManager.SpawnInsideItems(_insidePositions, transform, out _obstacles, out _rewards);
    }

    private void HideEnvironment() {
      HidePoolable(_outsideItems);
      HidePoolable(_obstacles);
      HidePoolable(_rewards);
    }

    private void HidePoolable<T>(List<T> list) where T : MonoBehaviour, IPoolable {
      if (list == null) {
        return;
      }

      foreach (T insideItem in list) {
        _objectPoolManager.ReturnToPool(insideItem);
      }

      list.Clear();
    }

    public Vector3 EnterPosition { get { return _enterPosition.position; } }
    public Vector3 ExitPosition { get { return _exitPosition.position; } }

    public void Initialize() {
      DiContainer container = ProjectContext.Instance.Container;
      _roadEnvironmentSpawnManager = container.Resolve<IRoadEnvironmentSpawnManager>();
      _objectPoolManager = container.Resolve<IObjectPoolManager>();
      ReturnToPool();
    }

    public void OnGetFromPool() {
      gameObject.SetActive(true);
      InPool = false;
    }

    public void ReturnToPool() {
      HideEnvironment();
      gameObject.SetActive(false);
      InPool = true;
    }

    public bool InPool {
      get;
      set;
    }
  }
}
