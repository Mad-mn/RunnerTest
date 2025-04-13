using System;
using System.Collections.Generic;

namespace Core.SaveLoadDataSystem.SavedData {
  [Serializable]
  public class PlayerData : BaseSavedData {
    private readonly List<int> _gamesTotalPoints;

    public PlayerData() {
      _gamesTotalPoints = new List<int>();
    }

    public List<int> GetListOfGames() {
      return _gamesTotalPoints;
    }

    public void SetupNewResult(int totalPoints) {
      _gamesTotalPoints.Add(totalPoints);
    }
  }
}
