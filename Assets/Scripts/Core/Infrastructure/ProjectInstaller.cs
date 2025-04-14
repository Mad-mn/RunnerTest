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
      Container.BindInterfacesAndSelfTo<ConfigManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<ObjectPoolManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<DataHandler>().AsSingle();
      Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }

    private void BindManagers() {
      Container.Bind<IRoadEnvironmentSpawnManager>().To<RoadEnvironmentSpawnManager>().AsSingle();
    }

    private void BindHandlers() {
      Container.Bind<IInputHandler>().To<InputHandler>().AsSingle();
    }
  }
}
