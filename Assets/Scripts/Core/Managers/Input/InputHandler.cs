namespace Core.Managers.Input {
  public class InputHandler : IInputHandler {

    public void OnLeftButton() {
      LeftButtonPressed = true;
    }

    public void OnRightButton() {
      RightButtonPressed = true;
    }

    public void ResetInputNextFrame() {
      LeftButtonPressed = false;
      RightButtonPressed = false;
    }

    public bool LeftButtonPressed {
      get ;
      private set;
    }

    public bool RightButtonPressed {
      get ;
      private set;
    }
  }
}
