using Core.SaveLoadDataSystem.SavedData;

namespace Core.SaveLoadDataSystem {
  public class DataHandler : IDataHandler {

    private ISaveSystem _saveSystem;
    private SaveData _saveData;

    public void Initialize() {
      _saveSystem = new SaveSystem();
      _saveSystem.Initialize();
      LoadData();
    }

    public T GetData<T>() where T : BaseSavedData {
      return _saveData.GetData<T>();
    }

    public void Save() {
      _saveSystem.Save(_saveData);
    }

    private void LoadData() {
      _saveData = _saveSystem.Load();
      if (_saveData == null) {
        InitializeData();
        _saveSystem.Save(_saveData);
      }
    }

    private void InitializeData() {
      _saveData = new SaveData();
      _saveData.Initialize();
    }
  }
}
