using Configs;
using Core.Loaders.Scene;
using Core.ObjectPool;
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

    private async void Start() {
      await InitializeItems();
      OnLoadComplete();
    }

    private async UniTask InitializeItems() {
      await _configManager.Initialize();
      await _poolManager.Initialize();
    }

    private async void OnLoadComplete() {
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
    }
  }
}
