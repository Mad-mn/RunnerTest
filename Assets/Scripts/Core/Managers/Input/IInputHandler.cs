namespace Core.Managers.Input {
  public interface IInputHandler {
    void OnLeftButton();
    void OnRightButton();
    void ResetInputNextFrame();
    bool LeftButtonPressed { get; }
    bool RightButtonPressed { get; }
  }
}
