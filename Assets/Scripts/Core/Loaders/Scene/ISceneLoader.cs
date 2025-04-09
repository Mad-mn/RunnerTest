using Cysharp.Threading.Tasks;

namespace Core.Loaders.Scene {
  public interface ISceneLoader {
    UniTask LoadSceneAsync(string sceneKey);
  }
}
