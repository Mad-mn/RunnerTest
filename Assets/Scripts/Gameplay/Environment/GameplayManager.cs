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
using Cysharp.Threading.Tasks;
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
    private readonly List<RoadItem> _roadItems = new List<RoadItem>();
    private PlayerController _playerController;
    private GameplayWindow _gameplayWindow;
    private ISceneLoader _sceneLoader;
    private IUIManager _uiManager;

    private void Awake() {
      _roadCreator = new RoadCreator(_objectPoolManager, _roadRoot);
      Initialize();
    }

    private async void Start() {
      await SpawnPlayer();
      InitializeStartedEnvironment();
      AddListeners();
    }

    private void OnDestroy() {
      RemoveListeners();
    }

    private void InitializeStartedEnvironment() {
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
      _roadItems.RemoveAt(0);
      _objectPoolManager.ReturnToPool(oldRoad);
      RoadItem newRoad = _roadCreator.SpawnRoadItem( _roadItems[^1].ExitPosition);
      newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
      newRoad.SetupEnvironment(true);
      _roadItems.Add(newRoad);
    }

    private async UniTask SpawnPlayer() {
      GameObject playerPrefab = _playerConfig.PlayerReference.Asset == null ? await _playerConfig.PlayerReference.LoadAssetAsync<GameObject>() : _playerConfig.PlayerReference.Asset as GameObject;
      GameObject player = Instantiate(playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
      _playerController = player.GetComponent<PlayerController>();
      _playerController.Initialize(new PlayerMovementData {
        ChangeSideSpeed = _playerConfig.ChangeSideSpeed,
        Speed = _playerConfig.RunSpeed,
        SideWight = _roadConfig.SideWight
      });
      player.transform.position = _playerSpawnPoint.position;
      _cameraController.SetupTarget(player.transform, _playerConfig.CameraOffset);
    }

    private void ClearScene() {
      foreach (RoadItem roadItem in _roadItems) {
        _objectPoolManager.ReturnToPool(roadItem);
      }
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
