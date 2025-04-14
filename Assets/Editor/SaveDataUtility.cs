using System.IO;
using Tools.Constants;
using UnityEditor;
using UnityEngine;

namespace Editor {
  public static class SaveDataUtility {
    private static string SaveFilePath {
      get {
        return Path.Combine(Application.persistentDataPath, Other.SaveDatFileName);
      }
    }

    [MenuItem("Tools/Clear Save Data")]
    public static void ClearSaveData() {
      if (File.Exists(SaveFilePath)) {
        File.Delete(SaveFilePath);
      }
    }
  }
}
