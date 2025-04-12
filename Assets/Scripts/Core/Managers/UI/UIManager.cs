using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.UIConfigs;
using Cysharp.Threading.Tasks;
using UI.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Managers.UI {
  public class UIManager : IUIManager {

    private readonly IConfigManager _configManager;
    private UIConfig _uiConfig;
    private List<BaseUIWindow> _windows;
    private GameObject _uiRoot;

    public UIManager(IConfigManager configManager) {
      _configManager = configManager;
    }

    public async UniTask Initialize() {
      _uiConfig = _configManager.GetConfig<UIConfig>();
      await CreateRoot();
      await InitializeWindows();
    }

    public T ShowWindow<T>() where T : BaseUIWindow {
      BaseUIWindow window = _windows.FirstOrDefault(window => window.GetType() == typeof(T));
      if (window != default) {
        window.Show();
        return window as T;
      }

      Debug.LogError($"Window {typeof(T)} exist");
      return null;
    }

    public void HideWindow<T>() where T : BaseUIWindow {
      BaseUIWindow window = _windows.FirstOrDefault(window => window.GetType() == typeof(T));
      if (window != default) {
        window.Hide();
        return;
      }

      Debug.LogError($"Window {typeof(T)} exist");
    }

    private async UniTask CreateRoot() {
      GameObject prefab = await _uiConfig.UIRoot.LoadAssetAsync<GameObject>();
      _uiRoot = Object.Instantiate(prefab);
      Object.DontDestroyOnLoad(_uiRoot);
    }

    private async UniTask InitializeWindows() {
      _windows = new List<BaseUIWindow>();
      foreach (AssetReference uiAssetReference in _uiConfig.UIAssetReferences) {
        GameObject windowPrefab = await uiAssetReference.LoadAssetAsync<GameObject>();
        GameObject windowObj = Object.Instantiate(windowPrefab, _uiRoot.transform);
        BaseUIWindow window = windowObj.GetComponent<BaseUIWindow>();
        window.Hide();
        _windows.Add(window);
      }
    }
  }
}
