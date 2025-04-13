using Core.Managers.Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Gameplay {
  public class InputPanel : MonoBehaviour {
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;

    private IInputHandler _inputHandler;

    private void Start() {
      InitComponents();
    }

    private void OnEnable() {
      AddListeners();
    }

    private void OnDisable() {
      RemoveListeners();
    }

    private void OnLeft() {
      _inputHandler.OnLeftButton();
    }

    private void OnRight() {
      _inputHandler.OnRightButton();
    }

    private void InitComponents() {
      _inputHandler = ProjectContext.Instance.Container.Resolve<IInputHandler>();
    }

    private void AddListeners() {
      _leftButton.onClick.AddListener(OnLeft);
      _rightButton.onClick.AddListener(OnRight);
    }

    private void RemoveListeners() {
      _leftButton.onClick.RemoveListener(OnLeft);
      _rightButton.onClick.RemoveListener(OnRight);
    }
  }
}
