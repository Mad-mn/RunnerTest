using Tools.Constants;
using UnityEngine;

namespace Player {
  public class PlayerAnimationController : MonoBehaviour {
    [SerializeField]
    private Animator _animator;

    public void StartRunAnimation() {
      _animator.SetBool(AnimationNames.PlayerRun, true);
    }
  }
}
