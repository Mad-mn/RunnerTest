using System;
using Core.Managers.Input;
using UnityEngine;

namespace Player {
  public class PlayerMovementController : MonoBehaviour {
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private PlayerAnimationController _animationController;

    private IInputHandler _inputHandler;

    private float _speed;
    private float _sideWight;
    private float _changeSideSpeed;

    private Vector3 _moveDirection;
    private int _currentLane;
    private bool _canRun;

    public void SetupData(PlayerMovementData movementData, IInputHandler inputHandler) {
      _inputHandler = inputHandler;
      _speed = movementData.Speed;
      _changeSideSpeed = movementData.ChangeSideSpeed;
      _sideWight = movementData.SideWight;
      _currentLane = 0;
    }

    public void StartRun() {
      _canRun = true;
      _animationController.StartRunAnimation();
    }

    public void Stop() {
      _canRun = false;
      _animationController.PlayIdleAnimation();
    }

    private void Update() {
      if (!_canRun) {
        return;
      }

      CheckInputs();
      Move();
    }

    private void CheckInputs() {
      bool inputLeft;
      bool inputRight;
#if UNITY_EDITOR
      inputLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || _inputHandler.LeftButtonPressed;
      inputRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || _inputHandler.RightButtonPressed;
#else
      inputLeft = _inputHandler.LeftButtonPressed;
      inputRight = _inputHandler.RightButtonPressed;
#endif
      if (inputLeft) {
        if (_currentLane > -1) {
          _currentLane--;
        }
      } else if (inputRight) {
        if (_currentLane < 1) {
          _currentLane++;
        }
      }
    }

    private void Move() {
      float targetX = _currentLane * _sideWight;
      float newX = Mathf.MoveTowards(transform.position.x, targetX, _changeSideSpeed);
      float xMovement = newX - transform.position.x;
      float zMovement = _speed * Time.deltaTime;

      _moveDirection = new Vector3(xMovement, 0, zMovement);
      _characterController.Move(_moveDirection);
    }

  }

  [Serializable]
  public struct PlayerMovementData {
    public float Speed;
    public float ChangeSideSpeed;
    public float SideWight;
  }
}
