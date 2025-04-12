using UnityEngine;

namespace Configs.RewardConfigs {
  [CreateAssetMenu(menuName = "Configs/Reward/RewardConfig", fileName = "RewardConfig")]
  public class RewardConfig : ScriptableObject {}

  public enum RewardType {
    Apple,
    Orange,
    Kiwi
  }
}
