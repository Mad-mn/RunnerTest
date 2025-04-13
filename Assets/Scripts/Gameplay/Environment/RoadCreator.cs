using Core.ObjectPool;
using Environment.Road;
using UnityEngine;

namespace Gameplay.Environment {
  public class RoadCreator : IRoadCreator {
    private readonly IObjectPoolManager _objectPoolManager;
    private readonly Transform _roadRoot;

    public RoadCreator(IObjectPoolManager objectPoolManager, Transform root) {
      _objectPoolManager = objectPoolManager;
      _roadRoot = root;
    }

    public RoadItem SpawnRoadItem(Vector3 entryPosition) {
      RoadItem roadItem = _objectPoolManager.GetFromPool<RoadItem>();
      roadItem.transform.SetParent(_roadRoot);
      SetupRoadPosition(entryPosition, roadItem);
      return roadItem;
    }

    private void SetupRoadPosition(Vector3 entryPosition, RoadItem newRoad) {
      Vector3 delta = entryPosition - newRoad.EnterPosition;
      newRoad.transform.position += delta;
    }
  }
}
