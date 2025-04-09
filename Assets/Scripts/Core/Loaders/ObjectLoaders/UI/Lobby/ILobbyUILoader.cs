using Cysharp.Threading.Tasks;

namespace Core.Loaders.ObjectLoaders.UI.Lobby {
  public interface ILobbyUILoader {
    UniTask<T> LoadObject<T>();
  }
}
