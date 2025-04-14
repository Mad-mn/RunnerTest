using Core.SaveLoadDataSystem.SavedData;

namespace Core.SaveLoadDataSystem {
  public interface IDataHandler {
    T GetData<T>() where T : BaseSavedData;

    void Save();
  }
}
