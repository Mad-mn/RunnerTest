using Cysharp.Threading.Tasks;

namespace Core.Loaders.AssetLoaders {
  public interface IAssetLoader {
    UniTask Initialize();
    int LoadAssetOrder { get; }
  }
}
