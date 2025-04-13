using Configs;
using Core.Loaders.Scene;
using Core.Managers.Input;
using Core.Managers.RoadEnvironment;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.SaveLoadDataSystem;
using Tools;
using Zenject;

namespace Core.Infrastructure {
  public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
      BindLoaders();
      BindManagers();
      BindHandlers();
      Container.BindInterfacesTo<GameTickHandler>().AsSingle();
    }

    private void BindLoaders() {
      Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }

    private void BindManagers() {
      Container.Bind<ConfigManager>().AsSingle();
      Container.Bind<IConfigManager>().To<ConfigManager>().FromResolve();
      Container.Bind<IObjectPoolManager>().To<ObjectPoolManager>().AsSingle();
      Container.Bind<IRoadEnvironmentSpawnManager>().To<RoadEnvironmentSpawnManager>().AsSingle();
      Container.Bind<IUIManager>().To<UIManager>().AsSingle();
    }

    private void BindHandlers() {
      Container.Bind<IDataHandler>().To<DataHandler>().AsSingle();
      Container.Bind<IInputHandler>().To<InputHandler>().AsSingle();
    }
  }
}
