using System.Collections.Generic;
using Camera;
using Configs;
using Configs.PlayerConfigs;
using Configs.RoadConfigs;
using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.Views.Gameplay;
using Core.Views.Lobby;
using Environment.Road;
using Player;
using Tools.Constants;
using UnityEngine;
using Zenject;

namespace Gameplay.Environment {
  public class GameplayManager : MonoBehaviour {
    [SerializeField]
    private Transform _playerSpawnPoint;
    [SerializeField]
    private Transform _startRoadSpawnPoint;
    [SerializeField]
    private Transform _roadRoot;
    [SerializeField]
    private GameplayCameraController _cameraController;

    [Inject]
    private IObjectPoolManager _objectPoolManager;
    [Inject]
    private IConfigManager _configManager;

    private PlayerConfigData _playerConfig;
    private RoadConfigData _roadConfig;

    private RoadCreator _roadCreator;
    private List<RoadItem> _roadItems;
    private PlayerController _playerController;
    private GameplayWindow _gameplayWindow;
    private ISceneLoader _sceneLoader;
    private IUIManager _uiManager;

    private void Awake() {
      _roadCreator = new RoadCreator(_objectPoolManager, _roadRoot);
      Initialize();
    }

    private async void Start() {
      InitializeStartedEnvironment();
      SpawnPlayer();
      AddListeners();
    }

    private void OnDestroy() {
      RemoveListeners();
    }

    private void InitializeStartedEnvironment() {
      _roadItems = new List<RoadItem>();
      int startAmount = _configManager.GetConfig<RoadConfig>().RoadConfigData.StartAmount;
      for (int i = 0; i < startAmount; i++) {
        RoadItem newRoad = _roadCreator.SpawnRoadItem(i == 0 ? _startRoadSpawnPoint.position : _roadItems[^1].ExitPosition);
        newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
        newRoad.SetupEnvironment(i != 0);
        _roadItems.Add(newRoad);
      }
    }

    private void OnPlayerEnterInNewRoadItem(RoadItem enteredRoad) {
      if (enteredRoad != _roadItems[0]) {
        UpdateRoad();
      }
    }

    private void UpdateRoad() {
      RoadItem oldRoad = _roadItems[0];
      oldRoad.OnPlayerEnter -= OnPlayerEnterInNewRoadItem;
      _roadItems.RemoveAt(0);
      _objectPoolManager.ReturnToPool(oldRoad);
      RoadItem newRoad = _roadCreator.SpawnRoadItem( _roadItems[^1].ExitPosition);
      newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
      newRoad.SetupEnvironment(true);
      _roadItems.Add(newRoad);
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
      foreach (RoadItem roadItem in _roadItems) {
        roadItem.OnPlayerEnter -= OnPlayerEnterInNewRoadItem;
        _objectPoolManager.ReturnToPool(roadItem);
      }

      _playerController.transform.position = _playerSpawnPoint.position;
      _objectPoolManager.ReturnToPool(_playerController);
    }

    private void Initialize() {
      _playerConfig = _configManager.GetConfig<PlayerConfig>().ConfigData;
      _roadConfig = _configManager.GetConfig<RoadConfig>().RoadConfigData;
      DiContainer container = ProjectContext.Instance.Container;
      _gameplayWindow = container.Resolve<IUIManager>().GetWindow<GameplayWindow>();
      _sceneLoader = container.Resolve<ISceneLoader>();
      _uiManager = container.Resolve<IUIManager>();
    }

    private void AddListeners() {
      _playerController.OnCatchReward += _gameplayWindow.OnPlayerCatchReward;
      _playerController.OnCollideWithObstacle += _gameplayWindow.OnPlayerCollideWithObstacle;
      _gameplayWindow.OnExit += ExitToLobby;
    }

    private void RemoveListeners() {
      _playerController.OnCatchReward -= _gameplayWindow.OnPlayerCatchReward;
      _playerController.OnCollideWithObstacle -= _gameplayWindow.OnPlayerCollideWithObstacle;
      _gameplayWindow.OnExit -= ExitToLobby;
    }

    private async void ExitToLobby() {
      ClearScene();
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.LobbySceneKey);
      _uiManager.HideWindow<GameplayWindow>();
      _uiManager.ShowWindow<LobbyWindow>();
    }
  }
}
