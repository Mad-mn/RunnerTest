using System.Collections.Generic;
using Camera;
using Configs;
using Configs.PlayerConfigs;
using Configs.RewardConfigs;
using Configs.RoadConfigs;
using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.SaveLoadDataSystem;
using Core.Views.Gameplay;
using Core.Views.Lobby;
using Environment.Road;
using Gameplay.Environment;
using Gameplay.Rewards;
using Player;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Gameplay {
  public class GameplayManager : MonoBehaviour {
    [SerializeField]
    private Transform _playerSpawnPoint;
    [SerializeField]
    private Transform _startRoadSpawnPoint;
    [SerializeField]
    private Transform _roadRoot;
    [SerializeField]
    private GameplayCameraController _cameraController;

    private IObjectPoolManager _objectPoolManager;
    private IConfigManager _configManager;
    private ISceneLoader _sceneLoader;
    private IUIManager _uiManager;
    private IDataHandler _dataHandler;
    private PlayerConfigData _playerConfig;
    private RoadConfigData _roadConfig;

    private IEnvironmentManager _environmentManager;
    private IGameplayRewardHandler _gameplayRewardHandler;

    private List<RoadItem> _roadItems;
    private PlayerController _playerController;
    private GameplayWindow _gameplayWindow;

    private void Awake() {
      InitializeComponents();
      CreateManagers();
    }

    private void CreateManagers() {
      _environmentManager = new EnvironmentManager(_objectPoolManager, _roadRoot, _startRoadSpawnPoint);
      _gameplayRewardHandler = new GameplayRewardHandler(_dataHandler, _configManager.GetConfig<RewardConfig>());
    }

    private void Start() {
      _environmentManager.InitializeStartedEnvironment(_roadConfig.StartAmount);
      SpawnPlayer();
      AddListeners();
    }

    private void OnDestroy() {
      RemoveListeners();
    }

    private void SpawnPlayer() {
      _playerController = _objectPoolManager.GetFromPool<PlayerController>();
      _playerController.Initialize(new PlayerMovementData {
        ChangeSideSpeed = _playerConfig.ChangeSideSpeed,
        Speed = _playerConfig.RunSpeed,
        SideWight = _roadConfig.SideWight
      });
      _playerController.transform.position = _playerSpawnPoint.position;
      _playerController.transform.SetParent(_playerSpawnPoint);
      _cameraController.SetupTarget(_playerController.transform, _playerConfig.CameraOffset);
    }

    private void ClearScene() {
      _environmentManager.Clear();

      _playerController.transform.position = _playerSpawnPoint.position;
      _objectPoolManager.ReturnToPool(_playerController);
    }

    private void InitializeComponents() {
      DiContainer container = ProjectContext.Instance.Container;
      _gameplayWindow = container.Resolve<IUIManager>().GetWindow<GameplayWindow>();
      _sceneLoader = container.Resolve<ISceneLoader>();
      _uiManager = container.Resolve<IUIManager>();
      _objectPoolManager = container.Resolve<IObjectPoolManager>();
      _configManager = container.Resolve<IConfigManager>();
      _dataHandler = container.Resolve<IDataHandler>();
      _playerConfig = _configManager.GetConfig<PlayerConfig>().ConfigData;
      _roadConfig = _configManager.GetConfig<RoadConfig>().RoadConfigData;
    }

    private void AddListeners() {
      _playerController.OnCatchReward += _gameplayWindow.OnPlayerCatchReward;
      _playerController.OnCatchReward += _gameplayRewardHandler.IncreaseRewards;
      _playerController.OnCollideWithObstacle += _gameplayWindow.OnPlayerCollideWithObstacle;
      _gameplayWindow.OnExit += ExitToLobby;
    }

    private void RemoveListeners() {
      _playerController.OnCatchReward -= _gameplayWindow.OnPlayerCatchReward;
      _playerController.OnCatchReward -= _gameplayRewardHandler.IncreaseRewards;
      _playerController.OnCollideWithObstacle -= _gameplayWindow.OnPlayerCollideWithObstacle;
      _gameplayWindow.OnExit -= ExitToLobby;
    }

    private async void ExitToLobby() {
      _gameplayRewardHandler.Save();
      ClearScene();
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
      _uiManager.HideWindow<GameplayWindow>();
      _uiManager.ShowWindow<LobbyWindow>();
    }
  }
}
