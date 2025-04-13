using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveDataUtility {
  private static string SaveFilePath {
    get {
      return Path.Combine(Application.persistentDataPath, "Save.dat");
    }
  }

  [MenuItem("Tools/Clear Save Data")]
  public static void ClearSaveData() {
    if (File.Exists(SaveFilePath)) {
      File.Delete(SaveFilePath);
    }
  }
}
