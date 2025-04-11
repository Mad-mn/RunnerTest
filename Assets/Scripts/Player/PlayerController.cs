using UnityEngine;

namespace Player {
  public class PlayerController : MonoBehaviour {
    [SerializeField]
    private PlayerMovementController _movementController;

    public void Initialize(PlayerMovementData movementData) {
      _movementController.SetupData(movementData);
    }
  }
}
