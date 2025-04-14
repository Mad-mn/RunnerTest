using Core.Loaders.AssetLoaders;
using Core.SaveLoadDataSystem.SavedData;
using Cysharp.Threading.Tasks;

namespace Core.SaveLoadDataSystem {
  public class DataHandler : IDataHandler, IAssetLoader {

    private ISaveSystem _saveSystem;
    private SaveData _saveData;

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

    UniTask IAssetLoader.Initialize() {
      _saveSystem = new SaveSystem();
      _saveSystem.Initialize();
      LoadData();
      return UniTask.CompletedTask;
    }

    public int LoadAssetOrder {
      get { return 1; }
    }
  }
}
