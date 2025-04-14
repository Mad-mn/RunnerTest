using System.Collections.Generic;
using System.Linq;
using Core.Loaders.AssetLoaders;
using Core.Loaders.Scene;
using Core.Managers.UI;
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
    private IUIManager _uiManager;
    [Inject]
    private List<IAssetLoader> _assetLoaders;

    private async void Start() {
      await InitializeItems();
      OnLoadComplete();
    }

    private async UniTask InitializeItems() {
      float startTime = Time.time;
      foreach (IAssetLoader assetLoader in _assetLoaders.OrderBy(x => x.LoadAssetOrder)) {
        await assetLoader.Initialize();
      }

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
