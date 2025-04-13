using System;
using System.Collections.Generic;
using System.Linq;
using Core.SaveLoadDataSystem.SavedData;
using UnityEngine;

namespace Core.SaveLoadDataSystem {
  [Serializable]
  public class SaveData {
    private List<BaseSavedData> _savedData;

    public void Initialize() {
      _savedData = new List<BaseSavedData> {
        new PlayerData()
      };
    }

    public T GetData<T>() where T : BaseSavedData {
      BaseSavedData data = _savedData.FirstOrDefault(data => data is T);
      if (data != default) {
        return data as T;
      }

      Debug.LogError($"Data type {typeof(T)} exist");
      return null;
    }
  }
}
