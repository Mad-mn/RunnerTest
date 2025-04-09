using Core.Loaders.Scene;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.Infrastructure {
  public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
      Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }
  }
}
