using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.RoadConfigs;
using Core.ObjectPool;
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

    public List<BaseOutsideItem> SpawnOutside(List<Transform> outsidePositions, Transform root) {
      List<BaseOutsideItem> spawnedItems = new List<BaseOutsideItem>();
      float chanceForSpawn = Random.Range(_roadConfigData.SpawnOutsideChanceMin, _roadConfigData.SpawnOutsideChanceMax);
      int spawnAmount = Mathf.RoundToInt(outsidePositions.Count*chanceForSpawn);
      Vector3 [] positions = new Vector3 [spawnAmount];
      Vector3 spawnPosition;
      for (int i = 0; i < spawnAmount; i++) {
        BaseOutsideItem randomItem = GetRandomSpawnItem();
        BaseOutsideItem item = _poolManager.GetFromPool<BaseOutsideItem>(randomItem.GetType());
        spawnPosition = GetRandomSpawnPosition(outsidePositions, positions);
        positions[i] = spawnPosition;
        item.transform.SetParent(root);
        item.transform.position = spawnPosition;
        item.transform.rotation = Quaternion.Euler(Vector3.up*Random.Range(0, 361));
        spawnedItems.Add(item);
      }

      return spawnedItems;
    }

    private BaseOutsideItem GetRandomSpawnItem() {
      return _roadConfigData.OutsideItems[Random.Range(0, _roadConfigData.OutsideItems.Count)];
    }

    private Vector3 GetRandomSpawnPosition(List<Transform> outsidePositions, Vector3[] positions) {
      List<Vector3> availablePositions = outsidePositions.Select(t => t.position).Where(pos => !positions.Contains(pos)).ToList();

      if (availablePositions.Count == 0) {
        Debug.LogWarning("Avalaible positions in emtpy");
        return Vector3.zero;
      }

      return availablePositions[Random.Range(0, availablePositions.Count)];
    }
  }
}
