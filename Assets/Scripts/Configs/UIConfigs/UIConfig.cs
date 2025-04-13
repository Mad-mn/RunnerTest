using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs.UIConfigs {
  [CreateAssetMenu (menuName = "Configs/UI/UIConfig", fileName = "UIConfig")]
  public class UIConfig : ScriptableObject {
    [SerializeField]
    private AssetReference _uiRoot;
    [SerializeField]
    private List<AssetReference> _uiAssets;

    public List<AssetReference> UIAssetReferences {
      get {
        return _uiAssets;
      }
    }

    public AssetReference UIRoot { get { return _uiRoot; } }
  }
}
