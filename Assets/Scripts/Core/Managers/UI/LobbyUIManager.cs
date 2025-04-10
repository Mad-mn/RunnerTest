using Core.Loaders.ObjectLoaders.UI.Lobby;
using UnityEngine;
using Zenject;

namespace Core.Managers.UI {
  public class LobbyUIManager : IUIManager {

    private readonly DiContainer _container;
    private readonly ILobbyUILoader _lobbyUILoader;
    private RectTransform _parent;
    private GameObject _currentUI;

    public LobbyUIManager(ILobbyUILoader lobbyUILoader) {
      _container = ProjectContext.Instance.Container;
      _lobbyUILoader = lobbyUILoader;
    }

    public async void ShowUI(RectTransform parent) {
      _parent = parent;
      GameObject prefab = await _lobbyUILoader.LoadObject<GameObject>();

      _currentUI = _container.InstantiatePrefab(prefab, _parent);
    }
  }
}
