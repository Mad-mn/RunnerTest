using System.Collections.Generic;
using Environment.OutsideObjects;
using UnityEngine;

namespace Core.Managers.RoadEnvironment {
  public interface IRoadEnvironmentSpawnManager {
    List<BaseOutsideItem> SpawnOutside(List<Transform> outsidePositions, Transform root);
  }
}
