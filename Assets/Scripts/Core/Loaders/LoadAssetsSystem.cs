using Configs;
using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.SaveLoadDataSystem;
using Core.Views.Lobby;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Core.Loaders {
  public class LoadAssetsSystem : MonoBehaviour {
    [Inject]
    private ISceneLoader _sceneLoader;
    [Inject]
    private ConfigManager _configManager;
    [Inject]
    private IObjectPoolManager _poolManager;
    [Inject]
    private IUIManager _uiManager;
    [Inject]
    private IDataHandler _dataHandler;

    private async void Start() {
      await InitializeItems();
      OnLoadComplete();
    }

    private async UniTask InitializeItems() {
      float startTime = Time.time;
      _dataHandler.Initialize();
      await _configManager.Initialize();
      await _poolManager.Initialize();
      await _uiManager.Initialize();
      float loadTime = Time.time - startTime;
      float timeToDelay = Other.MinimumLoaderTime - loadTime;
      if (timeToDelay < 0) {
        return;
      }

      await UniTask.Delay((int)(timeToDelay*1000), cancellationToken: destroyCancellationToken);
    }

    private async void OnLoadComplete() {
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
      _uiManager.ShowWindow<LobbyWindow>();
    }
  }
}
