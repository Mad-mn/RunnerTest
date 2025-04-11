using UnityEngine;

namespace Configs {
  public interface IConfigManager {
    T GetConfig<T>() where T : ScriptableObject;
  }
}
