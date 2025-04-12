using System;
using System.Collections.Generic;
using Environment.OutsideObjects;
using UnityEngine;

namespace Configs.RoadConfigs {
  [CreateAssetMenu(menuName = "Configs/RoadConfigs/RoadConfig", fileName = "RoadConfig")]
  public class RoadConfig : ScriptableObject {
    [SerializeField]
    private RoadConfigData _roadConfigData;

    public RoadConfigData RoadConfigData {
      get {
        return _roadConfigData;
      }
    }
  }

  [Serializable]
  public struct RoadConfigData {
    public int StartAmount;
    [Range(0, 1)]
    public float SpawnOutsideChanceMax;
    [Range(0, 1)]
    public float SpawnOutsideChanceMin;
    public List<BaseOutsideItem> OutsideItems;
  }
}
