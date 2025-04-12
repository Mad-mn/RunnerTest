using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Core.Loaders.Scene {
  public class SceneLoader : ISceneLoader {
    public async UniTask LoadSceneAsync(string sceneKey) {
      await Addressables.LoadSceneAsync(sceneKey);
    }
  }
}
