using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.SaveLoadDataSystem {
  public class BinarySaveSystem {
    private readonly string _filePath;

    public BinarySaveSystem() {
      _filePath = Path.Combine(Application.persistentDataPath, "Save.dat");
    }

    public void Save(SaveData data) {
      using (FileStream file = File.Create(_filePath)) {
        new BinaryFormatter().Serialize(file, data);
      }
    }

    public SaveData Load() {
      SaveData saveData;
      if (!File.Exists(_filePath)) {
        return null;
      }

      using (FileStream file = File.Open(_filePath, FileMode.Open)) {
        object loadedData = new BinaryFormatter().Deserialize(file);
        saveData = (SaveData)loadedData;
      }

      return saveData;
    }
  }
}
