using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs.PoolConfigs {
  [CreateAssetMenu(menuName = "Configs/PoolConfigs/BasePoolConfig", fileName = "BasePoolConfig")]
  public class BasePoolConfig : ScriptableObject {
    [SerializeField]
    private List<PoolPrefabItem> _prefabsForPool;

    public List<PoolPrefabItem> Prefabs { get { return _prefabsForPool; } }
  }

  [Serializable]
  public class PoolPrefabItem {
    public GameObject Prefab;
    public int SpawnAmount;
  }

}
