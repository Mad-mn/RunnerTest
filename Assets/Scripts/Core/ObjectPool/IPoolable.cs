namespace Core.ObjectPool {
  public interface IPoolable {
    void Initialize();
    void OnGetFromPool();
    void ReturnToPool();

    bool InPool { get; set; }
  }
}
