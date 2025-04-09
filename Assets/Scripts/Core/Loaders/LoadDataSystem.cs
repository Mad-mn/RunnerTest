using System;
using Core.Loaders.Scene;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Core.Loaders {
    public class LoadDataSystem : MonoBehaviour {
        [Inject]
        private ISceneLoader _sceneLoader;
        
        private void Start() {
            OnLoadComplete();
        }

        private async void OnLoadComplete() {
            await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
        }
    }
}
