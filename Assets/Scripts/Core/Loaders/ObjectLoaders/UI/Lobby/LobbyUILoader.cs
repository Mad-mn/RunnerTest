using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Core.Loaders.ObjectLoaders.UI.Lobby {
  public class LobbyUILoader : ILobbyUILoader {
    private const string PATH = "UI/LobbyUI.prefab";

    public async UniTask<T> LoadObject<T>() {
      return await Addressables.LoadAssetAsync<T>(PATH);
    }
  }
}
