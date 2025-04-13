using Core.SaveLoadDataSystem.SavedData;

namespace Core.SaveLoadDataSystem {
  public interface IDataHandler {
    void Initialize();
    T GetData<T>() where T : BaseSavedData;

    void Save();
  }
}
