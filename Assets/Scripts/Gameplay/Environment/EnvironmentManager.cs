using System.Collections.Generic;
using Core.ObjectPool;
using Environment.Road;
using UnityEngine;
using Zenject;

namespace Gameplay.Environment {
  public class EnvironmentManager : MonoBehaviour {
    [SerializeField]
    private Transform _startRoadSpawnPoint;
    [SerializeField]
    private Transform _roadRoot;

    [Inject]
    private IObjectPoolManager _objectPoolManager;

    private RoadCreator _roadCreator;
    private readonly List<RoadItem> _roadItems = new List<RoadItem>();

    private void Awake() {
      _roadCreator = new RoadCreator(_objectPoolManager, _roadRoot);
    }

    private void Start() {
      InitializeStartedEnvironment();
    }

    private void InitializeStartedEnvironment() {
      for (int i = 0; i < 3; i++) {
        RoadItem newRoad = _roadCreator.SpawnRoadItem(i == 0 ? _startRoadSpawnPoint.position : _roadItems[^1].ExitPosition);
        newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
        _roadItems.Add(newRoad);
      }
    }

    private void OnPlayerEnterInNewRoadItem(RoadItem enteredRoad) {
      if (enteredRoad != _roadItems[0]) {
        UpdateRoad();
      }
    }

    private void UpdateRoad() {
      RoadItem oldRoad = _roadItems[0];
      _roadItems.RemoveAt(0);
      _objectPoolManager.ReturnToPool(oldRoad);
      RoadItem newRoad = _roadCreator.SpawnRoadItem( _roadItems[^1].ExitPosition);
      newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
      _roadItems.Add(newRoad);
    }
  }
}
