using System;
using UnityEngine;

namespace Configs.PlayerConfigs {
  [CreateAssetMenu(menuName = "Configs/PlayerConfigs/PlayerConfig", fileName = "PlayerConfig")]
  public class PlayerConfig : ScriptableObject {
    [SerializeField]
    private PlayerConfigData _playerConfigData;

    public PlayerConfigData ConfigData {
      get { return _playerConfigData; }
    }

  }

  [Serializable]
  public struct PlayerConfigData {
    public string PlayerPrefabPath;
    public Vector3 CameraOffset;
    public float RunSpeed;
    public float ChangeSideSpeed;
  }
}
