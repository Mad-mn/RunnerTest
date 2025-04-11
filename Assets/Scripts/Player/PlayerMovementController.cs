using UnityEngine;

namespace Player {
  public class PlayerMovementController : MonoBehaviour {
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private float _forwardSpeed = 5f;
    [SerializeField]
    private float _laneDistance = 2f;
    [SerializeField]
    private float _laneChangeSpeed = 10f;

    private Vector3 _moveDirection;
    private int _currentLane  ;

    private bool _start;

    private void Update() {

      if (Input.GetKeyDown(KeyCode.W)) {
        _start = true;
      }

      if (!_start) {
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

      float targetX = _currentLane * _laneDistance;
      float newX = Mathf.MoveTowards(transform.position.x, targetX, _laneChangeSpeed);
      float xMovement = newX - transform.position.x;
      float zMovement = _forwardSpeed * Time.deltaTime;

      _moveDirection = new Vector3(xMovement, 0, zMovement);
      _characterController.Move(_moveDirection);
    }

  }
}
