using Core.Loaders.ObjectLoaders.UI.Lobby;
using Core.Managers.UI;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure {
  public class LobbySceneInstaller : MonoInstaller {
    [SerializeField]
    private RectTransform _uiRoot;

    public override void InstallBindings() {
      Container.Bind<ILobbyUILoader>().To<LobbyUILoader>().AsSingle();
      Container.Bind<IUIManager>().To<LobbyUIManager>().AsSingle();
    }

    public override void Start() {
      IUIManager uiManager = Container.Resolve<IUIManager>();
      uiManager.ShowUI(_uiRoot);
    }
  }
}
