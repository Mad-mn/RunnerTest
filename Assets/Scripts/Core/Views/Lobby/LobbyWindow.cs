using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.Views.Gameplay;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Views.Lobby {
  public class LobbyWindow : BaseUIWindow {
    [SerializeField]
    private Button _playButton;

    private IUIManager _uiManager;
    private ISceneLoader _sceneLoader;

    private void Start() {
      InitComponents();
    }

    private void OnEnable() {
      AddListeners();
    }

    private void OnDisable() {
      RemoveListeners();
    }

    private void OnPlayButton() {
      LoadPlayScene().Forget();
      _uiManager.HideWindow<LobbyWindow>();
      _uiManager.ShowWindow<GameplayWindow>();
    }

    private async UniTaskVoid LoadPlayScene() {
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.GameplaySceneKey);
    }

    private void AddListeners() {
      _playButton.onClick.AddListener(OnPlayButton);
    }

    private void RemoveListeners() {
      _playButton.onClick.RemoveListener(OnPlayButton);
    }

    private void InitComponents() {
      DiContainer container = ProjectContext.Instance.Container;
      _sceneLoader = container.Resolve<ISceneLoader>();
      _uiManager = container.Resolve<IUIManager>();
    }
  }
}
