using System.Collections.Generic;
using Camera;
using Configs;
using Configs.PlayerConfigs;
using Core.ObjectPool;
using Cysharp.Threading.Tasks;
using Environment.Road;
using Player;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    private RoadCreator _roadCreator;
    private readonly List<RoadItem> _roadItems = new List<RoadItem>();
    private PlayerController _playerController;

    private void Awake() {
      _roadCreator = new RoadCreator(_objectPoolManager, _roadRoot);
    }

    private void Start() {
      SpawnPlayer().Forget();
      InitializeStartedEnvironment();
    }

    private void InitializeStartedEnvironment() {
      for (int i = 0; i < 3; i++) {
        RoadItem newRoad = _roadCreator.SpawnRoadItem(i == 0 ? _startRoadSpawnPoint.position : _roadItems[^1].ExitPosition);
        newRoad.OnPlayerEnter += OnPlayerEnterInNewRoadItem;
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
      _roadItems.Add(newRoad);
    }

    private async UniTaskVoid SpawnPlayer() {
      _playerConfig = _configManager.GetConfig<PlayerConfig>().ConfigData;

      GameObject playerPrefab = await Addressables.LoadAssetAsync<GameObject>(_playerConfig.PlayerPrefabPath);
      GameObject player = Instantiate(playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
      _playerController = player.GetComponent<PlayerController>();
      _playerController.Initialize(new PlayerMovementData {
        ChangeSideSpeed = _playerConfig.ChangeSideSpeed,
        Speed = _playerConfig.RunSpeed,
        SideWight = 2
      });
      player.transform.position = _playerSpawnPoint.position;
      _cameraController.SetupTarget(player.transform, _playerConfig.CameraOffset);
    }
  }
}
