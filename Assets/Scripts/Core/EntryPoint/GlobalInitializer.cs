using Core.Loaders.Scene;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Core.EntryPoint {
  public class GlobalInitializer : MonoBehaviour {
    
    private void Start() {
      EnterInGame();
    }

    private async void EnterInGame() {
      var sceneLoader = ProjectContext.Instance.Container.Resolve<ISceneLoader>();
       await sceneLoader.LoadSceneAsync(SceneNameConstants.LoadingSceneKey);
    }
  }
}
