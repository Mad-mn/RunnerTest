using Environment.Road;
using UnityEngine;

namespace Gameplay.Environment {
  public interface IRoadCreator {
    RoadItem SpawnRoadItem(Vector3 entryPosition);
  }
}
