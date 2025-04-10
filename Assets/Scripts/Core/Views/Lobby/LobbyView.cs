using Core.Loaders.Scene;
using Tools.Constants;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Views.Lobby {
  public class LobbyView : MonoBehaviour {
    [SerializeField]
    private Button _playButton;

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
      _sceneLoader.LoadSceneAsync(SceneNameConstants.GameplaySceneKey);
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
    }
  }
}
