using System.Collections.Generic;
using Core.ObjectPool;
using Environment.Road;
using UnityEngine;

namespace Gameplay.Environment {
  public class EnvironmentManager : IEnvironmentManager {
    private readonly IRoadCreator _roadCreator;
    private readonly IObjectPoolManager _objectPoolManager;
    private readonly Transform _startRoadSpawnPoint;
    private List<RoadItem> _roadItems;

    public EnvironmentManager(IObjectPoolManager objectPoolManager, Transform roadRoot, Transform startRoadSpawnPoint) {
      _roadCreator = new RoadCreator(objectPoolManager, roadRoot);
      _startRoadSpawnPoint = startRoadSpawnPoint;
      _objectPoolManager = objectPoolManager;
    }

    public void InitializeStartedEnvironment(int startAmount) {
      _roadItems = new List<RoadItem>();
      for (int i = 0; i < startAmount; i++) {
        RoadItem newRoad = _roadCreator.SpawnRoadItem(i == 0 ? _startRoadSpawnPoint.position : _roadItems[^1].ExitPosition);
        newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
        newRoad.SetupEnvironment(i != 0);
        _roadItems.Add(newRoad);
      }
    }

    public void Clear() {
      foreach (RoadItem roadItem in _roadItems) {
        roadItem.OnPlayerEnter -= OnPlayerEnterInNewRoadItem;
        _objectPoolManager.ReturnToPool(roadItem);
      }
    }

    private void OnPlayerEnterInNewRoadItem(RoadItem enteredRoad) {
      if (enteredRoad != _roadItems[0]) {
        UpdateRoad();
      }
    }

    private void UpdateRoad() {
      RoadItem oldRoad = _roadItems[0];
      oldRoad.OnPlayerEnter -= OnPlayerEnterInNewRoadItem;
      _roadItems.RemoveAt(0);
      _objectPoolManager.ReturnToPool(oldRoad);
      RoadItem newRoad = _roadCreator.SpawnRoadItem( _roadItems[^1].ExitPosition);
      newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
      newRoad.SetupEnvironment(true);
      _roadItems.Add(newRoad);
    }
  }
}
