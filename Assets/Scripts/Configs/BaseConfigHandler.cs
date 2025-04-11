using System.Collections.Generic;
using UnityEngine;

namespace Configs {
  [CreateAssetMenu(menuName = "Configs/Handler", fileName = "ConfigsHandler")]
  public class BaseConfigHandler : ScriptableObject {
    [SerializeField]
    private List<ScriptableObject> _configs;

    public List<ScriptableObject> Configs { get { return _configs; } }
  }
}
