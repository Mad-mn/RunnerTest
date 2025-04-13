using System;
using UnityEngine;

namespace Player {
  public class PlayerMovementController : MonoBehaviour {
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private PlayerAnimationController _animationController;

    private float _speed;
    private float _sideWight;
    private float _changeSideSpeed;

    private Vector3 _moveDirection;
    private int _currentLane  ;

    private bool _canRun;

    public void SetupData(PlayerMovementData movementData) {
      _speed = movementData.Speed;
      _changeSideSpeed = movementData.ChangeSideSpeed;
      _sideWight = movementData.SideWight;
    }

    public void Stop() {
      _canRun = false;
    }

    private void Update() {

      if (Input.GetKeyDown(KeyCode.W)) {
        _canRun = true;
        _animationController.StartRunAnimation();
      }

      if (!_canRun) {
        return;
      }

      if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
        if (_currentLane > -1) {
          _currentLane--;
        }
      } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
        if (_currentLane < 1) {
          _currentLane++;
        }
      }

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
