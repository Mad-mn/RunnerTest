using Configs;
using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.Views.Lobby;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Core.Loaders {
  public class LoadDataSystem : MonoBehaviour {
    [Inject]
    private ISceneLoader _sceneLoader;
    [Inject]
    private ConfigManager _configManager;
    [Inject]
    private IObjectPoolManager _poolManager;
    [Inject]
    private IUIManager _uiManager;

    private async void Start() {
      await InitializeItems();
      OnLoadComplete();
    }

    private async UniTask InitializeItems() {
      await _configManager.Initialize();
      await _poolManager.Initialize();
      await _uiManager.Initialize();
    }

    private async void OnLoadComplete() {
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
      _uiManager.ShowWindow<LobbyWindow>();
    }
  }
}
