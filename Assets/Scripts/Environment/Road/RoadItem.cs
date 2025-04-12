using System;
using System.Collections.Generic;
using Core.Managers.RoadEnvironment;
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

    private IRoadEnvironmentSpawnManager _roadEnvironmentSpawnManager;

    private List<BaseOutsideItem> _outsideItems;

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag(TagLayerNames.PlayerTag)) {
        OnPlayerEnter?.Invoke(this);
      }
    }

    private void SetupEnvironment() {
      _outsideItems = _roadEnvironmentSpawnManager.SpawnOutside(_outsidePositions, transform);
    }

    private void HideEnvironment() {
      if (_outsideItems != null) {
        foreach (BaseOutsideItem outsideItem in _outsideItems) {
          outsideItem.OnSetToPool();
        }
      }
    }

    public Vector3 EnterPosition { get { return _enterPosition.position; } }
    public Vector3 ExitPosition { get { return _exitPosition.position; } }

    public void Initialize() {
      _roadEnvironmentSpawnManager = ProjectContext.Instance.Container.Resolve<IRoadEnvironmentSpawnManager>();
      OnSetToPool();
    }

    public void OnGetFromPool() {
      gameObject.SetActive(true);
      InPool = false;
      SetupEnvironment();
    }

    public void OnSetToPool() {
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
