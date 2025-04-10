using Cysharp.Threading.Tasks;

namespace Core.Loaders.ObjectLoaders {
  public interface IObjectLoader {
    UniTask<T> LoadObject<T>();
  }
}
