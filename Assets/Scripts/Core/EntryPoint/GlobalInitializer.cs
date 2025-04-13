using Core.Loaders.Scene;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Core.EntryPoint {
  public class GlobalInitializer : MonoBehaviour {

    private void Start() {
      Application.targetFrameRate = 60;
      EnterInGame();
    }

    private async void EnterInGame() {
      var sceneLoader = ProjectContext.Instance.Container.Resolve<ISceneLoader>();
      await sceneLoader.LoadSceneAsync(SceneNameConstants.LoadingSceneKey);
    }
  }
}
