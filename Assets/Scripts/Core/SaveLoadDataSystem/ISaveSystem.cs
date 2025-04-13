namespace Core.SaveLoadDataSystem {
  public interface ISaveSystem {
    void Initialize();
    void Save(SaveData data);
    SaveData Load();
  }
}
