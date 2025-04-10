namespace Core.ObjectPool {
  public interface IPoolable {
    void Initialize();
    void OnGetFromPool();
    void OnSetToPool();

    bool InPool { get; set; }
  }
}
