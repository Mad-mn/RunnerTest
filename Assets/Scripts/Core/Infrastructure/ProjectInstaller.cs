using Configs;
using Core.Loaders.Scene;
using Core.Managers.RoadEnvironment;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.SaveLoadDataSystem;
using Zenject;

namespace Core.Infrastructure {
  public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
      Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
      Container.Bind<ConfigManager>().AsSingle();
      Container.Bind<IConfigManager>().To<ConfigManager>().FromResolve();
      Container.Bind<IObjectPoolManager>().To<ObjectPoolManager>().AsSingle();
      Container.Bind<IRoadEnvironmentSpawnManager>().To<RoadEnvironmentSpawnManager>().AsSingle();
      Container.Bind<IUIManager>().To<UIManager>().AsSingle();
      Container.Bind<IDataHandler>().To<DataHandler>().AsSingle();
    }
  }
}
