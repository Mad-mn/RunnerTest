using Core.Loaders.Scene;
using Zenject;

namespace Core.Infrastructure {
  public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
      Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }
  }
}
