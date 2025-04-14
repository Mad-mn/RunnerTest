using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.RoadConfigs;
using Core.ObjectPool;
using Environment.InsideObjects;
using Environment.InsideObjects.Obstacles;
using Environment.OutsideObjects;
using UnityEngine;

namespace Core.Managers.RoadEnvironment {
  public class RoadEnvironmentSpawnManager : IRoadEnvironmentSpawnManager {

    private readonly RoadConfigData _roadConfigData;
    private readonly IObjectPoolManager _poolManager;

    public RoadEnvironmentSpawnManager(IConfigManager configManager, IObjectPoolManager poolManager) {
      _roadConfigData = configManager.GetConfig<RoadConfig>().RoadConfigData;
      _poolManager = poolManager;
    }

    public void SpawnInsideItems(List<Transform> insidePositions, Transform root, out List<BaseInsideItem> obstacles, out List<BaseInsideItem> rewards) {
      Vector3[] takenPositions;
      obstacles = SpawnObstacle(insidePositions, root, out takenPositions);
      List<Transform> lastPositions = insidePositions.Select(transform => transform).Where(pos => !takenPositions.Contains(pos.position)).ToList();
      rewards = SpawnReward(lastPositions, root);
    }

    public List<BaseOutsideItem> SpawnOutside(List<Transform> outsidePositions, Transform root) {
      List<BaseOutsideItem> spawnedItems = new List<BaseOutsideItem>();
      int spawnAmount = CalculateSpawnAmount(outsidePositions.Count, _roadConfigData.SpawnOutsideChanceMin, _roadConfigData.SpawnOutsideChanceMax);
      Vector3[] takenPositions = new Vector3[spawnAmount];

      for (int i = 0; i < spawnAmount; i++) {
        BaseOutsideItem randomItem = GetRandomSpawnOutsideItem();
        BaseOutsideItem item = SpawnItem(randomItem, root, outsidePositions, takenPositions, i, true);
        spawnedItems.Add(item);
      }

      return spawnedItems;
    }

    private List<BaseInsideItem> SpawnObstacle(List<Transform> insidePositions, Transform root, out Vector3[] takenPositions) {
      List<BaseInsideItem> spawnedItems = new List<BaseInsideItem>();
      int spawnAmount = CalculateSpawnAmount(insidePositions.Count, _roadConfigData.SpawnObstacleChanceMin, _roadConfigData.SpawnObstacleChanceMax);
      takenPositions = new Vector3[spawnAmount];

      for (int i = 0; i < spawnAmount; i++) {
        BaseInsideItem randomItem = GetRandomSpawnObstacleItem();
        BaseInsideItem item = SpawnItem(randomItem, root, insidePositions, takenPositions, i);
        spawnedItems.Add(item);
      }

      return spawnedItems;
    }

    private List<BaseInsideItem> SpawnReward(List<Transform> insidePositions, Transform root) {
      List<BaseInsideItem> spawnedItems = new List<BaseInsideItem>();
      int spawnAmount = CalculateSpawnAmount(insidePositions.Count, _roadConfigData.SpawnRewardChanceMin, _roadConfigData.SpawnRewardChanceMax);
      Vector3[] takenPositions = new Vector3[spawnAmount];

      for (int i = 0; i < spawnAmount; i++) {
        BaseInsideItem randomItem = GetRandomSpawnRewardItem();
        BaseInsideItem item = SpawnItem(randomItem, root, insidePositions, takenPositions, i);
        spawnedItems.Add(item);
      }

      return spawnedItems;
    }

    private int CalculateSpawnAmount(int positionCount, float minChance, float maxChance) {
      float chance = Random.Range(minChance, maxChance);
      return Mathf.RoundToInt(positionCount * chance);
    }

    private void SetupTransform(Transform itemTransform, Transform parent, Vector3 position, bool randomRotation = false) {
      itemTransform.SetParent(parent);
      itemTransform.position = position;
      if (randomRotation) {
        itemTransform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
      }
    }

    private T SpawnItem<T>(T prefab, Transform root, List<Transform> possiblePositions, Vector3[] takenPositions, int index, bool randomRotation = false) where T : MonoBehaviour, IPoolable {
      T item = _poolManager.GetFromPool<T>(prefab.GetType());
      Vector3 spawnPosition = prefab is BaseObstacle ? GetRandomObstacleSpawnPosition(possiblePositions, takenPositions) : GetRandomSpawnPosition(possiblePositions, takenPositions);
      takenPositions[index] = spawnPosition;
      SetupTransform(item.transform, root, spawnPosition, randomRotation);
      return item;
    }

    private BaseOutsideItem GetRandomSpawnOutsideItem() {
      return _roadConfigData.OutsideItems[Random.Range(0, _roadConfigData.OutsideItems.Count)];
    }

    private BaseInsideItem GetRandomSpawnRewardItem() {
      return _roadConfigData.RewardItems[Random.Range(0, _roadConfigData.RewardItems.Count)];
    }

    private BaseInsideItem GetRandomSpawnObstacleItem() {
      return _roadConfigData.Obstacles[Random.Range(0, _roadConfigData.Obstacles.Count)];
    }

    private Vector3 GetRandomSpawnPosition(List<Transform> outsidePositions, Vector3[] positions) {
      List<Vector3> availablePositions = outsidePositions.Select(t => t.position).Where(pos => !positions.Contains(pos)).ToList();

      if (availablePositions.Count == 0) {
        Debug.LogWarning("Avalaible positions in emtpy");
        return Vector3.zero;
      }

      return availablePositions[Random.Range(0, availablePositions.Count)];
    }

    private Vector3 GetRandomObstacleSpawnPosition(List<Transform> outsidePositions, Vector3[] positions) {
      List<Vector3> occupiedPositions = new List<Vector3>(positions);
      Vector3 newPosition = Vector3.zero;

      int attempts = 0;
      int maxAttempts = 20;

      bool validPositionFound = false;

      do {
        newPosition = GetRandomSpawnPosition(outsidePositions, occupiedPositions.ToArray());
        int nearObstacleCount = occupiedPositions.Count(pos => pos.z == newPosition.z);

        if (nearObstacleCount < _roadConfigData.SideAmount - 1) {
          validPositionFound = true;
        } else {
          occupiedPositions.Add(newPosition);
        }

        attempts++;

      } while (!validPositionFound && attempts < maxAttempts);

      if (!validPositionFound) {
        Debug.LogError("Cant find valid position for obstacle");
      }

      return newPosition;
    }
  }
}
