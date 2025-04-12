using System.Collections.Generic;
using Camera;
using Configs;
using Configs.PlayerConfigs;
using Configs.RoadConfigs;
using Core.Managers.UI;
using Core.ObjectPool;
using Core.Views.Gameplay;
using Cysharp.Threading.Tasks;
using Environment.Road;
using Player;
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
      GameObject playerPrefab = await _playerConfig.PlayerReference.LoadAssetAsync<GameObject>();
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

    private void Initialize() {
      _playerConfig = _configManager.GetConfig<PlayerConfig>().ConfigData;
      _roadConfig = _configManager.GetConfig<RoadConfig>().RoadConfigData;
      _gameplayWindow = ProjectContext.Instance.Container.Resolve<IUIManager>().GetWindow<GameplayWindow>();
    }

    private void AddListeners() {
      _playerController.OnCatchReward += _gameplayWindow.OnPlayerCatchReward;
    }

    private void RemoveListeners() {
      _playerController.OnCatchReward += _gameplayWindow.OnPlayerCatchReward;
    }
  }
}
