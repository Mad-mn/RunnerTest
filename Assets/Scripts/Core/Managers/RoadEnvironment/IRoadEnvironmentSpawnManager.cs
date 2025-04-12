using System.Collections.Generic;
using Environment.InsideObjects;
using Environment.OutsideObjects;
using UnityEngine;

namespace Core.Managers.RoadEnvironment {
  public interface IRoadEnvironmentSpawnManager {
    List<BaseOutsideItem> SpawnOutside(List<Transform> outsidePositions, Transform root);
    void SpawnInsideItems(List<Transform> insidePositions, Transform root, out List<BaseInsideItem> obstacles, out List<BaseInsideItem> rewards);
  }
}
