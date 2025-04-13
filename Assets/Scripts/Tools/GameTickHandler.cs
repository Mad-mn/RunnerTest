using Core.Managers.Input;
using Zenject;

namespace Tools {
  public class GameTickHandler : ITickable {
    private readonly IInputHandler _inputHandler;

    public GameTickHandler(IInputHandler inputHandler) {
      _inputHandler = inputHandler;
    }

    public void Tick() {
      _inputHandler.ResetInputNextFrame();
    }
  }
}
