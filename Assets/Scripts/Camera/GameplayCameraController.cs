using UnityEngine;

namespace Camera {
  public class GameplayCameraController : MonoBehaviour {
    private Transform _target;
    private Vector3 _cameraOffset;

    private Coroutine _moveRoutine;

    public void SetupTarget(Transform player, Vector3 offset) {
      _target = player;
      _cameraOffset = offset;
    }

    private void LateUpdate() {
      if (_target != null) {
        Vector3 newPosition = new Vector3(_cameraOffset.x, _target.position.y + _cameraOffset.y, _target.position.z + _cameraOffset.z);
        transform.position = newPosition;
      }
    }
  }
}
