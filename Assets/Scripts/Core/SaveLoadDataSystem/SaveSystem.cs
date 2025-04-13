namespace Core.SaveLoadDataSystem {
  public class SaveSystem : ISaveSystem {

    private BinarySaveSystem _binarySaveSystem;
    private SaveData _saveData;

    public void Initialize() {
      _binarySaveSystem = new BinarySaveSystem();
    }

    public void Save(SaveData data) {
      _binarySaveSystem.Save(data);
    }

    public SaveData Load() {
      return _binarySaveSystem.Load();
    }
  }
}
