using System;
using System.Collections.Generic;
using Core.Loaders.AssetLoaders;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs {
  public class ConfigManager : IConfigManager, IAssetLoader {

    private readonly Dictionary<Type, ScriptableObject> _configs = new Dictionary<Type, ScriptableObject>();

    public async UniTask Initialize() {
      BaseConfigHandler handler = await Addressables.LoadAssetAsync<BaseConfigHandler>(ObjectsPath.ConfigHandler);
      List<ScriptableObject> configs = handler.Configs;
      foreach (ScriptableObject config in configs) {
        _configs.Add(config.GetType(), config);
      }
    }

    public int LoadAssetOrder {
      get { return 2; }
    }

    public T GetConfig<T>() where T : ScriptableObject {
      ScriptableObject config = _configs[typeof(T)];
      if (config != null) {
        return config as T;
      }

      Debug.Log($"Config type {typeof(T)} exist");
      return null;
    }
  }
}
