using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Windows {
  public abstract class BaseUIWindow : MonoBehaviour {

    public abstract UniTask Initialize();

    public virtual void Show() {
      gameObject.SetActive(true);
    }

    public virtual void Hide() {
      gameObject.SetActive(false);
    }
  }
}
