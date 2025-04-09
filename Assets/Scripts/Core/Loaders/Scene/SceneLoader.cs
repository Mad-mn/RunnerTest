using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Core.Loaders.Scene {
  public class SceneLoader : ISceneLoader {
    public async UniTask LoadSceneAsync(string sceneKey) {
      var handle = Addressables.LoadSceneAsync(sceneKey);
      await handle.ToUniTask();
    }
  }
}
